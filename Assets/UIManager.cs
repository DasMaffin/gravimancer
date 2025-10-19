using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
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
    public GameObject RespawnButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RespawnButton.SetActive(false);
    }

    public void toggleRespawnButton() 
    {
        RespawnButton.SetActive(!RespawnButton.activeSelf);
    }

    public void OnRespawnClicked()
    {
        toggleRespawnButton();
        LocalPlayerManager.MyLocalPlayerManager.SpawnPlayer();
    }
}
