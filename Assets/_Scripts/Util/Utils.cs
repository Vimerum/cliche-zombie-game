using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public static Vector2Int TruncateToInt(this Vector2 pos) {
        return new Vector2Int((int)pos.x, (int)pos.y);
    }
}
