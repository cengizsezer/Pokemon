using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtentions
{
    #region VECTOR3

    public static Vector2 XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 YZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 WithX(this Vector3 v, Vector3 _v) => WithX(v, _v.x);

    public static Vector3 WithX(this Vector3 v, Transform t) => WithX(v, t.position.x);

    public static Vector3 WithY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 WithY(this Vector3 v, Vector3 _v) => WithY(v, _v.y);

    public static Vector3 WithY(this Vector3 v, Transform t) => WithY(v, t.position.y);

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static Vector3 WithZ(this Vector3 v, Vector3 _v) => WithZ(v, _v.z);

    public static Vector3 WithZ(this Vector3 v, Transform t) => WithZ(v, t.position.z);

    public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v.x = x.HasValue ? x.Value : v.x;
        v.y = y.HasValue ? y.Value : v.y;
        v.z = z.HasValue ? z.Value : v.z;
        return v;
    }

    public static Vector3 X0Z(this Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }

    public static Vector3 XY0(this Vector3 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector3 Abs(Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static Vector3Int ToInt(this Vector3 v, bool willRound = false)
    {
        if (willRound)
        {
            return new Vector3Int(
                Mathf.RoundToInt(v.x),
                Mathf.RoundToInt(v.y),
                Mathf.RoundToInt(v.z)
                );
        }
        else
        {
            return new Vector3Int(
                Mathf.FloorToInt(v.x),
                Mathf.FloorToInt(v.y),
                Mathf.FloorToInt(v.z)
                );
        }
    }

    public static float DistanceToLine(this Vector3 v, Vector3 origin, Vector3 direction)
    {
        return Vector3.Cross(direction, v - origin).magnitude;
    }

    public static float DistanceToLine(this Vector3 v, Ray ray)
    {
        return v.DistanceToLine(ray.origin, ray.direction);
    }

    public static Vector3 NearestPointOnLine(this Vector3 v, Vector3 origin, Vector3 direction)
    {
        direction.Normalize();
        v = v - origin;
        var d = Vector3.Dot(v, direction);
        return origin + direction * d;
    }

    public static Vector3 NearestPointOnLine(this Vector3 v, Ray ray)
    {
        return NearestPointOnLine(v, ray.origin, ray.direction);
    }

    #endregion

    #region VECTOR2

    public static Vector3 XY0(this Vector2 v)
    {
        return new Vector3(v.x, v.y, 0f);
    }

    public static Vector3 X0Y(this Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public static float ToAngle(this Vector2 v)
    {
        float result = 0f;
        var dir = v.normalized;
        if (dir.x > 0)
        {
            result = Mathf.Asin(dir.y);
        }
        else if (dir.y > 0)
        {
            result = Mathf.Acos(dir.x);
        }
        else
        {
            var point = -dir.y;
            point = Mathf.Clamp(point, -1, 1);
            result = Mathf.Asin(point) + Mathf.PI;
        }

        result *= Mathf.Rad2Deg;

        return result;
    }

    public static float AngleTo(this Vector2 v1, Vector2 v2)
    {
        var angle1 = v1.ToAngle();
        var angle2 = v2.ToAngle();
        return angle2 - angle1;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, degrees));
        Vector3 result = matrix * v.XY0();
        return result.XY();
    }

    public static Vector2 Abs(this Vector2 v)
    {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    #endregion
}
