using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Tachyogenesis.Data;
using Tachyogenesis.Scripts.Entity.NPC;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts;

public partial class Player : CharacterBody2D
{
	/*
	 * ######
	 * FIELDS
	 * ######
	 */
	
	[Signal] public delegate void HealthChangedEventHandler(float health);

	// ----- NODES -----
	private AnimatedSprite2D sprite;
	private Controller controller;
	
	// ----- MOVEMENT -----
	public float Speed { get; private set; } = 300.0f;
	public float JumpVelocity { get; private set; }= -400.0f;
	private Vector2 direction = new(1, 0);
	private Vector2 reach = new(100, 0);
	private Vector2 origin;
	private CoordHist coordHist;
	
	// ----- COMBAT -----
	public float Health { get; private set; }
	public float MaxHealth { get; private set; } = 100f;
	public float BaseAttack { get; private set; } = 25;
	public float Pierce { get; private set; } = 1;
	
	// ----- TIME MANIPULATION ----
	private const float TimeScaleSnapZone = 0.25f;
	private float timeScaleTarget = 1f;
	private float heldTimeScaleOrigin;
	private float heldTimeScaleTarget;
	private bool heldTimeScaleTargetReached;
	private bool slowdownHeld;
	private bool speedupHeld;
	// you'd think this is easy

	/*
	 * #######
	 * METHODS
	 * #######
	 */
	
	// ----- GODOT -----
	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		origin = Position;
		coordHist = new CoordHist(origin);
		Health = MaxHealth; // in case controller isn't called for some reason
	}

	public override void _Process(double delta)
	{
		UpdateTimeScale(delta);
		ProcessCameraMovement();
		UpdateTargetOutline();
		UpdateDirection();
	}

	public override void _PhysicsProcess(double delta)
	{
		// if (controller.TimeScale < 0)
		// {
		// 	var snapshot = coordHist.GetLatestAtOrBefore(controller.Elapsed);
		// 	if (snapshot is { } position)
		// 		Position = new Vector2(position.x, position.y);
		// 	coordHist.RemoveAfter(controller.Elapsed);
		// 	Velocity = Vector2.Zero;
		// 	return;
		// }

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
		coordHist.Add(new CoordinatePair(Position.X, Position.Y, controller.Elapsed));

		if (Position.Y > 5000) Die();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			AttemptInteract();
		}

		if (@event.IsActionPressed("attack"))
		{
			Attack();
		}

		if (@event.IsActionPressed("time_down") && !slowdownHeld && !speedupHeld)
		{
			slowdownHeld = true;
			heldTimeScaleOrigin = GetTimeScaleStage(controller.TimeScale);
			heldTimeScaleTarget = heldTimeScaleOrigin > 0f ? 0f : -1f;
			heldTimeScaleTargetReached = false;
			timeScaleTarget = heldTimeScaleTarget;
		}

		if (@event.IsActionReleased("time_down") && slowdownHeld)
		{
			slowdownHeld = false;
			timeScaleTarget = heldTimeScaleTargetReached
				? heldTimeScaleTarget
				: heldTimeScaleOrigin;
			if (!heldTimeScaleTargetReached)
				controller.TimeScale = heldTimeScaleOrigin;
		}

		if (@event.IsActionPressed("time_up") && !speedupHeld && !slowdownHeld)
		{
			speedupHeld = true;
			heldTimeScaleOrigin = GetTimeScaleStage(controller.TimeScale);
			heldTimeScaleTarget = heldTimeScaleOrigin < 0f ? 0f : 1f;
			heldTimeScaleTargetReached = false;
			timeScaleTarget = heldTimeScaleTarget;
		}

		if (@event.IsActionReleased("time_up") && speedupHeld)
		{
			speedupHeld = false;
			timeScaleTarget = heldTimeScaleTargetReached
				? heldTimeScaleTarget
				: heldTimeScaleOrigin;
			if (!heldTimeScaleTargetReached)
				controller.TimeScale = heldTimeScaleOrigin;
		}
	}
	
	// ----- MOVEMENT -----
	private void UpdateDirection() => sprite.FlipH = direction.X < 0;

	private bool CanMove()
	{
		return !controller.InCutscene;
	}
	
	private void ProcessCameraMovement()
	{
		CoordinateRange clamps;
		try
		{
			clamps = Constants.MAX_CAMERA_CLAMPS[controller.CurrentMap];
		}
		catch (KeyNotFoundException)
		{
			controller.ErrorHelper.ThrowOnce(Errors.CameraClampsNotSet, this.ToString());
			clamps = new CoordinateRange();
		}

		controller.Camera.Position = new Vector2
		(
			Mathf.Clamp (Position.X, clamps.x1, clamps.x2),
			Mathf.Clamp (Position.Y, clamps.y1, clamps.y2)
		);
	}
	
	
	private void Die()
	{
		Position = Vector2.Zero;
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

	private Entity.Entity GetClosestTarget(List<Entity.Entity> targets)
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

	private List<Entity.Entity> GetInteractablesWithinReach()
	{
		List<Entity.Entity> targets = GetInteractables();
		List<Entity.Entity> targetsWithinReach = [];

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

	private List<Entity.Entity> GetInteractables()
	{
		List<Entity.Entity> targets = [];

		foreach (Entity.Entity e in GetTree().GetNodesInGroup("Interactable"))
		{
			targets.Add(e);
		}

		return targets;
	}

	private List<Enemy> GetEnemies()
	{
		List<Enemy> targets = [];

		foreach (Enemy e in GetTree().GetNodesInGroup("Enemy"))
		{
			targets.Add(e);
		}

		return targets;
	}

	private List<Enemy> GetEnemiesWithinReach()
	{
		List<Enemy> targets = GetEnemies();
		List<Enemy> targetsWithinReach = [];

		foreach (Enemy e in targets)
		{
			if (IsWithinReach(e))
			{
				targetsWithinReach.Add(e);
			}
		}

		return targetsWithinReach;
	}

	private List<Node2D> SortClosestToPlayer(List<Node2D> nodes) =>
		[.. nodes.OrderBy(node => node.Position.DistanceTo(this.Position))];

	private List<Enemy> SortClosestToPlayer(List<Enemy> nodes) =>
		[.. nodes.OrderBy(node => node.Position.DistanceTo(this.Position))];

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

	private static float GetTimeScaleStage(double scale)
	{
		float absDistanceToZero = Mathf.Abs((float)scale);
		float absDistanceToOne = Mathf.Abs((float)scale - 1f);
		float absDistanceToNegativeOne = Mathf.Abs((float)scale + 1f);

		if (absDistanceToZero <= absDistanceToOne && absDistanceToZero <= absDistanceToNegativeOne)
			return 0f;
		if (absDistanceToOne < absDistanceToNegativeOne)
			return 1f;
		return -1f;
	}

	private void UpdateTimeScale(double delta)
	{
		float t = 1 - Mathf.Exp(-2 * (float)delta);
		controller.TimeScale += (timeScaleTarget - controller.TimeScale) * t;

		if ((slowdownHeld || speedupHeld) &&
			Mathf.Abs(heldTimeScaleTarget - controller.TimeScale) <= TimeScaleSnapZone)
		{
			controller.TimeScale = heldTimeScaleTarget;
			heldTimeScaleTargetReached = true;
		}
	}

	public void SetHealth(float health, bool broadcast = true)
	{
		Health = Mathf.Clamp(health, 0, MaxHealth);
		if (broadcast) EmitSignalHealthChanged(Health);
		
		if (Health <= 0) Die();
	}

	public void ChangeHealth(float dHealth, bool broadcast = true) => SetHealth(Health + dHealth, broadcast);

	public void TakeDamage(float dmg)
	{
		float def = 0; // TODO: IMPLEMENT
		
		ChangeHealth(-(dmg - def));
	}

	public void Attack()
	{
		List<Enemy> targets = SortClosestToPlayer(GetEnemiesWithinReach());
		List<Enemy> targetsToHit = [];
		
		for (int i = 0; i < targets.Count; i++)
		{
			if (i >= Mathf.Floor(Pierce)) break;
			targetsToHit.Add(targets[i]);
		}

		foreach (Enemy e in targetsToHit)
		{
			e.TakeDamage(CalculateAttackDamage());
		}

	}

	// attack stuff goes here;
	public float CalculateAttackDamage()
	{
		return BaseAttack;
	}
}
