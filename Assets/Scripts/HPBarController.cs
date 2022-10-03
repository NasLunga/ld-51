using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public float maxOffset = 1.5f;
    public float yellowPoint = 0.5f;
    public float redPoint = 0.2f;
    public GameObject gauge;
    private float percents;
    
    public void setPercents(float p) {
        percents = p;
        // p = 1; o = 0;
        // p = 0.5; o = maxOffset / 2;
        // p = 0; o = maxOffset;
        float offset = -((1 - p) * maxOffset);
        gauge.transform.localPosition = new Vector3(offset, 0f, 0f);

        Color color;
        if (p > yellowPoint) {
            color = new Color(0f, 1f, 0f);
        } else if (p > redPoint) {
            color = new Color(1f, 1f, 0f);
        } else {
            color = new Color(1f, 1f, 0f);
        }
        gauge.GetComponent<SpriteRenderer>().color = color;
    }
}
