using UnityEngine;

[System.Serializable]
public class Terrain {
    [System.Serializable]
    public abstract class NoiseSettings {
        [ReadOnly]
        public float seed;

        public void GenerateSeed () {
            seed = Random.Range(-1000f, 1000f);
        }

        public virtual float Noise (float x, float y) {
            return Noise(new Vector2(x, y));
        }

        public virtual float Noise (Vector2 pos) {
            return Mathf.PerlinNoise(pos.x + seed, pos.y + seed);
        }
    }

    [System.Serializable]
    public class RiverSettings : NoiseSettings {
        public float thickness;
        public float density;
        public float strength;
        public float frequency;

        public override float Noise (Vector2 pos) {
            float noise = (Mathf.PerlinNoise(pos.x / frequency, 0f) * 2) - 1;

            return noise * strength;
        }
    }

    [System.Serializable]
    public class ResourceSettings : NoiseSettings {
        public float scale;
        public float threshold;

        public override float Noise (Vector2 pos) {
            return Mathf.PerlinNoise((pos.x * scale) + seed, (pos.y * scale) + seed);
        }

        public bool IsNoiseGreaterThanThreshold (float x, float y) {
            return IsNoiseGreaterThanThreshold(new Vector2(x, y));
        }

        public bool IsNoiseGreaterThanThreshold (Vector2 pos) {
            return Noise(pos) > threshold;
        }
    }
}
