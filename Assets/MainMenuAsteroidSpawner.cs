using System.Collections.Generic;
using UnityEngine;

public class MainMenuAsteroidSpawner : MonoBehaviour
{
    public Transform min;
    public Transform max;
    public List<GameObject> asteroids = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0f, 4f);
    }

    private void SpawnAsteroid()
    {
        GameObject go = Instantiate(
            asteroids[Random.Range(0, asteroids.Count - 1)], 
            new Vector3(this.transform.position.x, Random.Range(min.position.y, max.position.y), 0), 
            Quaternion.identity, this.transform);
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(20f, 80f), 0));
        rb.AddTorque(Random.Range(-100f, 100f));
    }
}
