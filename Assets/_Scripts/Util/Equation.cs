using UnityEngine;
using System.Collections;

public class Equation {

    private float m;
    private float b;
    private float angle;

    public Equation (Vector2 initial, Vector2 final) {
        m = (final.y - initial.y) / (final.x - initial.x);
        b = initial.y - (m * initial.x);

        angle = Mathf.Atan(m);
    }

    public float FindY (float x) {
        return m * x + b;
    }

    public Vector2 FindYWithNoise (float x) {
        float y = (Mathf.PerlinNoise(x, 0f) * 2) - 1;

        float xL = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
        float yL = x * Mathf.Sin(angle) + y * Mathf.Cos(angle) + b;

        Debug.Log("(" + x + ", " + y + ") => (" + xL + ", " + yL + ")");
        return new Vector2(xL, yL);
    }
}
