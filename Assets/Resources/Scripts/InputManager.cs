using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance
    {
        get => instance;
        set
        {
            if (instance != null)
            {
                Destroy(instance);
            }
            instance = value;
        }
    }

    public GameObject DrawnGravLinePrefab;
    public GameObject DrawnGravObjectPrefab;

    public Dictionary<ulong, GameObject> currentGravLineObject = new Dictionary<ulong, GameObject>(); 
    private Coroutine drawCoroutine;

    private bool _holdingM1 = false;
    public bool holdingM1
    {
        get => _holdingM1;
        private set
        {
            if (_holdingM1 == value)
                return;

            _holdingM1 = value;

            if (_holdingM1)
            {
                LocalPlayerManager.MyLocalPlayerManager.PlayerController.StartNewGravLine();

                drawCoroutine = StartCoroutine(LocalPlayerManager.MyLocalPlayerManager.PlayerController.DrawRoutine());
            }
            else
            {
                if (drawCoroutine != null)
                {
                    StopCoroutine(drawCoroutine);
                    drawCoroutine = null;

                    LocalPlayerManager.MyLocalPlayerManager.PlayerController.TryDrawOnce();
                }

                currentGravLineObject = new Dictionary<ulong, GameObject>();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void OnChangeGravity(InputValue value)
    {
        if (LocalPlayerManager.MyLocalPlayerManager == null || LocalPlayerManager.MyLocalPlayerManager.GravityImpact == null) return;
        float input = value.Get<Vector2>().y; // x would be press wheel left/right. Pos = up scroll, neg = down scroll
        LocalPlayerManager.MyLocalPlayerManager.GravityImpact.ChangeGravity(input);
    }

    public void OnAttack(InputValue value)
    {
        if (LocalPlayerManager.MyLocalPlayerManager == null || LocalPlayerManager.MyLocalPlayerManager.PlayerController == null) return;
        holdingM1 = value.isPressed;
    }
}
