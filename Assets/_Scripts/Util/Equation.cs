using UnityEngine;
using System.Collections;

public class Equation {

    private readonly float m;
    private readonly float b;
    private readonly Vector2 normal;

    public Equation (Vector2 initial, Vector2 final) {
        m = (final.y - initial.y) / (final.x - initial.x);
        b = initial.y - (m * initial.x);

        normal = Vector2.Perpendicular(final - initial).normalized;
    }

    public float FindY (float x) {
        return m * x + b;
    }

    public Vector2 FindYWithNoise (float x, float noiseStrength, float noiseFrequency) {
        Vector2 pos = new Vector2(x, FindY(x));

        float noise = (Mathf.PerlinNoise(x / noiseFrequency, 0f) * 2) - 1;
        pos += normal * noise * noiseStrength;

        return pos;
    }
}
