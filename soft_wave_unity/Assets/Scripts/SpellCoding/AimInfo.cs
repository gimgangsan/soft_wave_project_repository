using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AimInfo
{
    public Vector2 ShooterPos;
    public Vector2 MousePos;

    public AimInfo(Vector2 ShooterPos, Vector2 MousePos)
    {
        this.ShooterPos = ShooterPos;
        this.MousePos = MousePos;
    }

    public float CastAngle()
    {
        Vector2 TargetDir = this.NomarlizeInto(1);
        bool IsPositiveRadian = ShooterPos.y < MousePos.y;
        float Radian = Vector2.Angle(TargetDir, Vector2.right);
        return (IsPositiveRadian ? Radian : -Radian);
    }

    public Vector2 NomarlizeInto(float ThisLength)
    {
        return (MousePos - ShooterPos) * ThisLength;
    }
}
