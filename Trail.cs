using Godot;
using System;

public partial class Trail : GpuParticles3D
{
    public override void _Ready()
    {
        Emitting = true;

        var particleMesh = new TubeTrailMesh();
        SetDrawPassMesh(0, particleMesh);
        particleMesh.Radius = 0.01f;
        particleMesh.SectionLength = 0.005f;
        particleMesh.CapTop = false;
        particleMesh.CapBottom = false;

        var processMaterial = new ParticleProcessMaterial();
        SetProcessMaterial(processMaterial);
        processMaterial.Gravity = Vector3.Zero;
        processMaterial.Color = new Color(1, 0, 0, 1);

        Amount = 300; 
        Lifetime = 5.0f;
        SpeedScale = 1.0f;
        Explosiveness = 0.0f;

    }

}