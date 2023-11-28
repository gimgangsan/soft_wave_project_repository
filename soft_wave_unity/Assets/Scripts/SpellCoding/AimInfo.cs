using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AimInfo
{
    public Transform ShooterTransform;
    public Vector2 MousePos;

    public AimInfo(Transform ShooterTransform, Vector2 MousePos)
    {
        this.ShooterTransform = ShooterTransform;
        this.MousePos = MousePos;
    }

    public float CastAngle()
    {
        Vector2 TargetDir = this.NomarlizeInto(1);
        bool IsPositiveRadian = ShooterTransform.position.y < MousePos.y;
        float Radian = Vector2.Angle(TargetDir, Vector2.right);
        return (IsPositiveRadian ? Radian : -Radian);
    }

    public Vector2 NomarlizeInto(float ThisLength)
    {
        return (MousePos - (Vector2)ShooterTransform.position) * ThisLength;
    }
}
