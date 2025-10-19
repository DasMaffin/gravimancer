using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float maxLifetime;
    private float lifetime;
    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime) Destroy(this.gameObject);
    }
}
