using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static Vector2 VectorToSingularDirection(Vector2 vector) {
        vector.Normalize();

        int x = 0;
        int y = 0;
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y)) {
            if (vector.x > 0) {
                x = 1;
            } else {
                x = -1;
            }
        } else {
            if (vector.y > 0) {
                y = 1;
            } else {
                y = -1;
            }
        }
        return new Vector2(x, y);
    }
}
