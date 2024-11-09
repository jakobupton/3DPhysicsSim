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
		// Referenced from : https://www.cfm.brown.edu/people/dobrush/am33/Mathematica/ch3/modify.html#:~:text=Modified%20Euler%20method%20%2F%20Midpoint%20Method&text=This%20method%20reevaluates%20the%20slope,halfway%20through%20the%20line%20segment.
		// Convert delta to float and calculate half time step, used to smooth the movement
		float dt = (float)delta;
		float half_dt = dt / 2;
		
		// Equations referenced from https://web.mit.edu/jorloff/www/chaosTalk/double-pendulum/double-pendulum-en.html
		// Calculate initial angular accelerations (theta1_dd and theta2_dd)
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
		
		//Compute midpoint values for omega and theta used in Midpoint smoothing
		float omega1_half = omega1 + theta1_dd * half_dt;
		float omega2_half = omega2 + theta2_dd * half_dt;
		float theta1_half = theta1 + omega1 * half_dt;
		float theta2_half = theta2 + omega2 * half_dt;
		
		// Calculating angular accelerations at midpoint, same formulae as above, used for smoothing the simulation
		num1 = -gravity * (2 * mass1 + mass2) * Mathf.Sin(theta1_half);
		num2 = -mass2 * gravity * Mathf.Sin(theta1_half - 2 * theta2_half);
		num3 = -2 * Mathf.Sin(theta1_half - theta2_half) * mass2;
		num4 = omega2_half * omega2_half * length2 + omega1_half * omega1_half * length1 * Mathf.Cos(theta1_half - theta2_half);
		den1 = length1 * (2 * mass1 + mass2 - mass2 * Mathf.Cos(2 * theta1_half - 2 * theta2_half));
		float theta1_dd_mid = (num1 + num2 + num3 * num4) / den1;

		num5 = 2 * Mathf.Sin(theta1_half - theta2_half);
		num6 = omega1_half * omega1_half * length1 * (mass1 + mass2);
		num7 = gravity * (mass1 + mass2) * Mathf.Cos(theta1_half);
		num8 = omega2_half * omega2_half * length2 * mass2 * Mathf.Cos(theta1_half - theta2_half);
		den2 = length2 * (2 * mass1 + mass2 - mass2 * Mathf.Cos(2 * theta1_half - 2 * theta2_half));
		float theta2_dd_mid = (num5 * (num6 + num7 + num8)) / den2;
		
		// Update angular velocities and angles w.r.t. midpoint method
		omega1 += theta1_dd_mid * dt;
		omega2 += theta2_dd_mid * dt;
		theta1 += omega1 * dt;
		theta2 += omega2 * dt;

		// Apply rotation
 		GetNode<Node3D>("Arm1").Rotation = new Vector3(Rotation.X, Rotation.Y, theta1); // Rotate Arm1
		GetNode<Node3D>("Arm1/Arm2Pivot").Rotation = new Vector3(Rotation.X, Rotation.Y, theta2); // Rotate Arm2 relative to Arm1
	}
}
