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
	private Vector2 direction = new Vector2(1, 0);
	private Vector2 reach = new Vector2(100, 0);
	private Controller controller;
	private const float Speed = 300.0f;
	private const float JumpVelocity = -400.0f;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		ProcessCameraMovement();
		UpdateTargetOutline();
		UpdateDirection();
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

	private void UpdateDirection() => sprite.FlipH = direction.X < 0;

	private void Die()
	{
		Position = Vector2.Zero;
	}

	private bool CanMove()
	{
		return !controller.InCutscene;
	}
	
	private void ProcessCameraMovement()
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

	private void AttemptInteract()
	{
		if (controller.InCutscene) return;
		var targets = GetInteractablesWithinReach();
		
		if (targets.Count < 1) return;
		var closest = GetClosestTarget(targets);

		// ReSharper disable once PossibleNullReferenceException
		// closest will never be null
		closest.Interact();
	}

	private Entity.Entity GetClosestTarget(List<Node2D> targets)
	{
		Entity.Entity closest = null;
		foreach (var node in targets)
		{
			if (node is not Entity.Entity { Interactable: true } e) continue;
			if (closest == null || node.Position.DistanceTo(Position) < closest.Position.DistanceTo(Position))
			{
				closest = e;
			}
		}

		return closest;
	}

	private List<Node2D> GetInteractablesWithinReach()
	{
		List<Node2D> targets = GetInteractables();
		List<Node2D> targetsWithinReach = [];
		
		foreach (var target in targets)
		{
			if (IsWithinReach(target))
			{
				targetsWithinReach.Add(target);
			}
		}

		return targetsWithinReach;
	}

	///<summary>
	/// Checks if the object is in reach, and if the player is looking at the object.
	/// As the direction the player is looking is either positive or negative, and so is the difference of
	/// the two positions; one can conclude that the player is looking at the object if the direction
	/// multiplied by the distance to the object is positive.
	/// </summary>
	private bool IsWithinReach(Node2D target) => Mathf.Abs(target.Position.X - Position.X) <= reach.X &&
												 (target.Position.X - Position.X) * direction.X >= 0;

	private List<Node2D> GetInteractables()
	{
		List<Node2D> targets = [];

		foreach (Node2D node in GetTree().GetNodesInGroup("Interactable"))
		{
			targets.Add(node);
		}

		return targets;
	}

	private void UpdateTargetOutline()
	{
		var targets = GetInteractables();
		foreach (var target in targets)
		{
			if (target is not Entity.Entity e) continue;
			e.SetOutline(false);
		}

		if (targets.Count < 1) return;
		var closest = GetClosestTarget(targets);
		if (IsWithinReach(closest)) closest.SetOutline(true);
	}
}
