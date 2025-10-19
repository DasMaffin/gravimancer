using UnityEngine;

public class ServerStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<NetworkStart>().StartServer();
    }
}
