using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DrawnGravLineController : NetworkBehaviour
{
    public NetworkVariable<List<Vector3>> drawnGravObjects = new NetworkVariable<List<Vector3>>(new List<Vector3>(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private LineRenderer myLineRenderer;

    private float maxLifetime = 3f;
    private float lifetime = 0f;

    private void Awake()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        myLineRenderer.positionCount = drawnGravObjects.Value.Count;
        myLineRenderer.SetPositions(drawnGravObjects.Value.ToArray());

        if (IsServer) 
        {
            lifetime += Time.deltaTime;
            if(lifetime > maxLifetime)
            {
                //InputManager.Instance.currentGravLineObject.Remove(this.OwnerClientId);
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
