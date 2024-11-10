using Godot;
using System;

public partial class Grass : MultiMeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var grassMesh = GetNode<MultiMeshInstance3D>("/root/DoublePendulum/Grass");

		var material = new StandardMaterial3D();
    	material.AlbedoColor = new Color(0.2f, 0.8f, 0.2f);
		grassMesh.MaterialOverride = material;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
