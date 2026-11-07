using Godot;
using Tachyogenesis.Data;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts;

public partial class Player : CharacterBody2D
{
	public byte direction;
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

		Vector2 direction = CanMove() 
			? Input.GetVector("move_left", "move_right", "ui_down", "ui_up")
			: Vector2.Zero;
		
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
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
}
