using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance;
    public static SceneManager Instance
    {
        get => _instance;
        set
        {
            if(_instance != null)
            {
                Destroy(value);
                return;
            }
            _instance = value;
        }
    }

    public PlayerInput playerInput;

    private void Awake()
    {
        Instance = this;
    }
}
