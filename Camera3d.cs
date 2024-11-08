using Godot;
using System;

public partial class Camera3d : Camera3D
{
	// Horizontal angle of the camera (movement along the xz-plane)
	float _angle;

	// Vertical angle of the camera (movement along the y-axis)
	float _yangle;

	// Distance from the center
	const float DISTANCE = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Get user input
		if (Input.IsKeyPressed(Key.Escape)) 
        {
            GetTree().Quit();
        }
		var currentMousePosition = GetViewport().GetMousePosition();
        var relativeMousePosition = (GetViewport().GetVisibleRect().Size / 2) - currentMousePosition;
        Input.WarpMouse(GetViewport().GetVisibleRect().Size / 2);  // Keep mouse centered

		_angle -= relativeMousePosition.X * 0.01f;
		_yangle -= relativeMousePosition.Y * 0.01f;

		// Move camera
		Position = new Vector3(
			DISTANCE * Mathf.Cos(_angle),
			DISTANCE * Mathf.Tan(_yangle),
			DISTANCE * Mathf.Sin(_angle)
		);

		// Look at center
		LookAt(new Vector3(0, 0, 0));
	}
}
