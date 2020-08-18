using Godot;
using System;

public class Player : Area2D
{

	[Signal]
	public delegate void Hit();

	[Export]
	public int Speed = 400; // How fast the player will move (pixels/sec).

	private Vector2 _screenSize; // Size of the game window.

	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
		Hide(); // No player at the start
	}


	public override void _Process(float delta)
	{
		var velocity = new Vector2(); // The player's movement vector.
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite"); // Refernce to Node

		// Get input and adjust velocity based on that
		velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

		// To animate or not to animate
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}

		// Selecting the animation and flipping the sprite based on movement
		if (velocity.x != 0)
		{
			animatedSprite.Animation = "walk";
			animatedSprite.FlipH = velocity.x < 0;
		}
		else if (velocity.y != 0)
		{
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}

		// Moving the player
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(Position.y, 0, _screenSize.y)
		);
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnPlayerBodyEntered(PhysicsBody2D body)
	{
		Hide(); // Player disapears after being hit
		EmitSignal("Hit");
		// Only get hit once by disabling the collision shape
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("Disabled", true);
	}

}
