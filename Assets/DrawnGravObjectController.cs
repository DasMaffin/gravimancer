using Unity.Netcode;
using UnityEngine;

public class DrawnGravObjectController : NetworkBehaviour
{
    public DrawnGravLineController parent;

    // Update is called once per frame
    void Update()
    {
        if (IsServer && parent == null)
        {
            GetComponent<NetworkObject>().Despawn(); 
        }
    }
}
