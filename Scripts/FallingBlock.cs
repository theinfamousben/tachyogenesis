using Godot;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts;

public partial class FallingBlock : RigidBody2D
{
	private Controller controller;
	private Vector2 origin;
	public Vector2 Velocity { get; set; }
	public CoordHist CoordHist { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		origin = Position;
		CoordHist = new CoordHist(origin);
		Velocity = Vector2.Zero;
		controller = GetNode<Controller>("/root/Controller");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (controller.TimeScale > 0)
			CoordHist.Add(new CoordinatePair(Position.X, Position.Y, controller.Elapsed));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Mathf.IsZeroApprox(controller.TimeScale))
		{
			Freeze = true;
		}
		else if (controller.TimeScale < 0)
		{
			Freeze = true;
			var e = CoordHist.GetLatestAtOrBefore(controller.Elapsed);
			if (e is not { } p) return;

			SetRewoundPosition(new Vector2(p.x, p.y));
			CoordHist.RemoveAfter(controller.Elapsed);
		}
		else
		{
			if (Freeze)
			{
				LinearVelocity = Vector2.Zero;
				AngularVelocity = 0;
				Sleeping = false;
			}

			Freeze = false;
			GravityScale = (float)controller.TimeScale;
		}
	}

	private void SetRewoundPosition(Vector2 position)
	{
		Position = position;
		LinearVelocity = Vector2.Zero;
		AngularVelocity = 0;
		Sleeping = true;

		PhysicsServer2D.BodySetState(
			GetRid(),
			PhysicsServer2D.BodyState.Transform,
			GlobalTransform
		);
	}
}
