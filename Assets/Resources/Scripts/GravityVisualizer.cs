using System.Collections.Generic;
using UnityEngine;

public class GravityVisualizer : MonoBehaviour
{
    public List<GameObject> circles = new List<GameObject>();
    private float shrinkFactor = 1f;

    // Update is called once per frame
    void Update()
    {
        if (LocalPlayerManager.MyLocalPlayerManager == null || LocalPlayerManager.MyLocalPlayerManager.GravityImpact == null) return;
        foreach (GameObject circle in circles)
        {
            float shrinkBy = shrinkFactor * Time.deltaTime * LocalPlayerManager.MyLocalPlayerManager.GravityImpact.gravityForce.Value;
            circle.transform.localScale = new Vector3(circle.transform.localScale.x - shrinkBy, circle.transform.localScale.y - shrinkBy, 1f);
            if (circle.transform.localScale.x < 0.1f) circle.transform.localScale = new Vector3(circles.Count, circles.Count, 1f);
        }
    }
}
