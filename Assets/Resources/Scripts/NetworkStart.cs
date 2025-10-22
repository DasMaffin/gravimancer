using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkManager))]
public class NetworkStart : MonoBehaviour
{
    public TextMeshProUGUI ipInput;
    public GameObject startGamePanel;
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
        if (InputTextChecker.IsValidIP(ipInput.text))
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipInput.text, (ushort)7777);
        }
        else
        {
            Debug.LogWarning("No valid IP Address given. Continuing with default values!");
        }
        NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
        toggleStartGamePanel();
        NetworkManager.Singleton.StartClient();
    }

    private void OnTransportFailure()
    {
        toggleStartGamePanel();
    }

    public void StartServer() 
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.StartServer();
    }

    private void OnServerStarted()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void toggleStartGamePanel()
    {
        startGamePanel.SetActive(!startGamePanel.activeSelf);
    }
}
