using UnityEngine;

public static class Multilerp
{
    public static Vector3 MultilerpFunction(Vector3[] points, float t)
    {
        if (t >= 1)
        {
            return points[points.Length - 1];
        }
        if (t <= 0)
        {
            return points[0];
        }

        int v = Mathf.FloorToInt(t * (points.Length - 1f));

        Vector3 from = points[v];
        Vector3 to = points[v + 1];

        float m = t * (points.Length - 1f) - v;

        return Vector3.Lerp(from, to, m);
    }
}