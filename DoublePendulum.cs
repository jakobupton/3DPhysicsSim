using Godot;
using System;

public partial class DoublePendulum : Node3D
{
    // Properties of arms and blobs
    private float length1 = 2.0f;
    private float length2 = 2.0f;
    private float mass1 = 0.10f;
    private float mass2 = 0.10f;
    private float gravity = 9.8f;

    // Initial angles and angular velocities before any physics
    private float theta1 = Mathf.Pi / 3; // Angle of pendulum1
    private float theta2 = Mathf.Pi / 2; // Angle of pendulum2
    private float omega1 = 0.0f; // Angular velocity pendulum1
    private float omega2 = 0.0f; // Angular velocity pendulum2

    public override void _PhysicsProcess(double delta)
    {   
        // Equations referenced from https://web.mit.edu/jorloff/www/chaosTalk/double-pendulum/double-pendulum-en.html
        // Calculate angular accelerations (theta1_dd and theta2_dd)
        float num1 = -gravity * (2 * mass1 + mass2) * Mathf.Sin(theta1);
        float num2 = -mass2 * gravity * Mathf.Sin(theta1 - 2 * theta2);
        float num3 = -2 * Mathf.Sin(theta1 - theta2) * mass2;
        float num4 = omega2 * omega2 * length2 + omega1 * omega1 * length1 * Mathf.Cos(theta1 - theta2);
        float den1 = length1 * (2 * mass1 + mass2 - mass2 * Mathf.Cos(2 * theta1 - 2 * theta2));
        float theta1_dd = (num1 + num2 + num3 * num4) / den1; // angular accel of pendulum1

        float num5 = 2 * Mathf.Sin(theta1 - theta2);
        float num6 = omega1 * omega1 * length1 * (mass1 + mass2);
        float num7 = gravity * (mass1 + mass2) * Mathf.Cos(theta1);
        float num8 = omega2 * omega2 * length2 * mass2 * Mathf.Cos(theta1 - theta2);
        float den2 = length2 * (2 * mass1 + mass2 - mass2 * Mathf.Cos(2 * theta1 - 2 * theta2));
        float theta2_dd = (num5 * (num6 + num7 + num8)) / den2; //angular accel of pendulum2

        // Update angular velocities and angles
        omega1 += theta1_dd * (float)delta;
        omega2 += theta2_dd * (float)delta;
        theta1 += omega1 * (float)delta;
        theta2 += omega2 * (float)delta;

        // Apply rotation
 		GetNode<Node3D>("Arm1").Rotation = new Vector3(Rotation.X, Rotation.Y, theta1); // Rotate Arm1
        GetNode<Node3D>("Arm1/Arm2Pivot").Rotation = new Vector3(Rotation.X, Rotation.Y, theta2); // Rotate Arm2 relative to Arm1
    }
}
