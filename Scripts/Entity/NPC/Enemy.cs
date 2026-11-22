using Godot;
using Tachyogenesis.Scripts;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class Enemy : NPC
{
	[Export] public float DetectionRange { get; set; } = 300.0f;
	[Export] public float AttackRange { get; set; } = 40.0f;
	[Export] public float MoveSpeed { get; set; } = 100.0f;
	[Export] public float AttackCooldown { get; set; } = 1.0f;

	public bool HasTarget { get; private set; }

	private CoordHist coordHist;
	private double attackTimer;

	public Enemy()
	{
		NPCType = NPCType.Enemy;
	}

	public override void _Ready()
	{
		base._Ready();
		coordHist = new CoordHist(Position);
	}

	public override void _Process(double delta)
	{
		if (controller?.TimeScale > 0)
		{
			coordHist.Add(new CoordinatePair(Position.X, Position.Y, controller.Elapsed));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (controller?.Player == null || controller.InCutscene || Mathf.IsZeroApprox(controller.TimeScale))
		{
			HasTarget = false;
			return;
		}

        if (controller.TimeScale < 0)
		{
			HasTarget = false;
			var snapshot = coordHist.GetLatestAtOrBefore(controller.Elapsed);
			if (snapshot is not { } position) return;

			Position = new Vector2(position.x, position.y);
			coordHist.RemoveAfter(controller.Elapsed);
			return;
		}

		var physicsDelta = (float)delta;
		Velocity += GetGravity() * physicsDelta * (float)controller.TimeScale;
		attackTimer = Mathf.Max(0.0f, (float)(attackTimer - delta));
		ScoutPlayer();

		if (HasTarget)
		{
			TargetPlayer();
		}
		else
		{
			Velocity = new Vector2(
				Mathf.MoveToward(Velocity.X, 0.0f, MoveSpeed * physicsDelta),
				Velocity.Y);
		}

		if (HasTarget &&
			GlobalPosition.DistanceTo(controller.Player.GlobalPosition) <= AttackRange &&
			attackTimer <= 0.0)
		{
			Attack(controller.Player);
			attackTimer = Mathf.Max(0.01f, AttackCooldown);
		}

		MoveAndSlide();
	}

    protected virtual void ScoutPlayer()
	{
		HasTarget = GlobalPosition.DistanceTo(controller.Player.GlobalPosition) <=
					Mathf.Max(0.0f, DetectionRange);
	}

    protected virtual void TargetPlayer()
	{
		if (!HasTarget) return;

		var playerPosition = controller.Player.GlobalPosition;
		if (GlobalPosition.DistanceTo(playerPosition) > AttackRange)
		{
			Velocity = new Vector2(
				Mathf.MoveToward(
					Velocity.X,
					Mathf.Sign(playerPosition.X - GlobalPosition.X) * Mathf.Max(0.0f, MoveSpeed),
					Mathf.Max(0.0f, MoveSpeed) * (float)GetPhysicsProcessDeltaTime()),
				Velocity.Y);
		}
		else
		{
			Velocity = new Vector2(0.0f, Velocity.Y);
		}
	}

    protected virtual void Attack(Player player) { }
}
