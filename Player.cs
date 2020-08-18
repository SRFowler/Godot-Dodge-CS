using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public int Speed = 400; // How fast the player will move (pixels/sec).

    private Vector2 _screenSize; // Size of the game window.

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
    }


    public override void _Process(float delta)
    {
        var velocity = new Vector2(); // The player's movement vector.

        /////
        //  TODO: Fix all of this. Use Input.get_action_strength()
        //  or whatever the C# implementation is
        /////
        if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= 1;
        }

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        Position += velocity * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, _screenSize.x),
            y: Mathf.Clamp(Position.y, 0, _screenSize.y)
        );
    }
}
