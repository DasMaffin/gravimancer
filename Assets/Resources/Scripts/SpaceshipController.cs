using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipController : NetworkBehaviour
{
    public float acceleration = 5f;

    private bool _isAlive = true;
    [HideInInspector] public bool isAlive 
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
        }
    }

    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
