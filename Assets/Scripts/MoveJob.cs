using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Burst;

[BurstCompile]
public struct MoveJob : IJobParallelForTransform
{
    public NativeArray<Vector3> Positions;
    public NativeArray<Vector3> Velocities;
    public NativeArray<Vector3> Accelerations;
    public float DeltaTime;
    public float VelocityLimit;

    public void Execute(int index, TransformAccess transform)
    {
        var velocity = Velocities[index] + Accelerations[index] * DeltaTime;
        var direction = velocity.normalized;
        velocity = direction * Mathf.Clamp(velocity.magnitude, 1, VelocityLimit);
        transform.position += velocity * DeltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        Positions[index] = transform.position;
        Velocities[index] = velocity;
        Accelerations[index] = Vector3.zero;
    }
}
