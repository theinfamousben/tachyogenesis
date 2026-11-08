using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;
using Tachyogenesis.Data;
using Tachyogenesis.Scripts.Entity.NPC;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts;

public partial class Player : CharacterBody2D
{
	public Vector2 direction = new Vector2(1, 0);
	public Vector2 reach = new Vector2(50, 0);
	private Controller controller;
	private const float Speed = 300.0f;
	private const float JumpVelocity = -400.0f;

	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
	}

	public override void _Process(double delta)
	{
		ProcessCameraMovement();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor() && CanMove())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 dir = CanMove() 
			? Input.GetVector("move_left", "move_right", "ui_down", "ui_up")
			: Vector2.Zero;
		
		if (dir != Vector2.Zero)
		{
			velocity.X = dir.X * Speed;
			direction = dir;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		if (Position.Y > 5000) Die();
	}

	void Die()
	{
		Position = Vector2.Zero;
	}

	public bool CanMove()
	{
		return !controller.InCutscene;
	}
	
	public void ProcessCameraMovement()
	{
		var clamps = Constants.MAX_CAMERA_CLAMPS[controller.currentMap];
		controller.Camera.Position = new Vector2
		(
			Mathf.Clamp (Position.X, clamps.x1, clamps.x2),
			Mathf.Clamp (Position.Y, clamps.y1, clamps.y2)
		);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			AttemptInteract();
		}
	}

	public void AttemptInteract()
	{
		if (controller.InCutscene) return;
		List<Node2D> targets = [];

		foreach (Node2D node in GetTree().GetNodesInGroup("Interactable"))
		{
			// Checks if the object is in reach, and if the player is looking at the object.
			// As the direction the player is looking is either positive or negative, and so is the difference of
			// the two positions; one can conclude that the player is looking at the object if the direction
			// multiplied by the distance to the object is positive.
			if (node.Position.X - Position.X <= reach.X && (node.Position.X - Position.X) * direction.X >= 0)
			{
				targets.Add(node);
			}
		}

		if (targets.Count < 1) return;

		Entity.Entity closest = null;
		foreach (var node in targets)
		{
			if (node is not Entity.Entity { Interactable: true } e) continue;
			if (closest == null || node.Position.DistanceTo(Position) < closest.Position.DistanceTo(Position))
			{
				closest = e;
			}
		}

		// ReSharper disable once PossibleNullReferenceException
		// closest will never be null
		closest.Interact();
	}
}
