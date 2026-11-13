using Godot;

namespace Tachyogenesis.Scripts;

public partial class FallingBlock : RigidBody2D
{
	private Controller controller;
	private Vector2 origin;
	public Vector2 Velocity { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Velocity = Vector2.Zero;
		controller = GetNode<Controller>("/root/Controller");
		origin = Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (controller.TimeScale < 0)
		{
			Freeze = true;
			Position = Position.MoveToward(
				origin,
				Mathf.Max(LinearVelocity.Length(), 1.0f) * Mathf.Abs((float)controller.TimeScale) * (float)delta
			);
			LinearVelocity = Vector2.Zero;
			return;
		}

		Freeze = Mathf.IsZeroApprox((float)controller.TimeScale);
		GravityScale = (float)controller.TimeScale;
	}
}
