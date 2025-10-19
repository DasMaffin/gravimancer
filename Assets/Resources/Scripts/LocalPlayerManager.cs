using Unity.Netcode;
using UnityEngine;

public class LocalPlayerManager : NetworkBehaviour
{
    public GameObject playerShipPrefab;
    private static LocalPlayerManager instance;
    public static LocalPlayerManager MyLocalPlayerManager
    {
        get => instance;
        set
        {
            instance = value;
        }
    }
    public GravityImpact GravityImpact;
    public PlayerController PlayerController;

    public override void OnNetworkSpawn()
    {
        DontDestroyOnLoad(this.gameObject);
        if(IsLocalPlayer)
        {
            MyLocalPlayerManager = this;
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        print("Spawning Player for ClientId: " + this.OwnerClientId);
        SpawnPlayerServerRpc(this.OwnerClientId);
    }

    [ServerRpc(RequireOwnership = true)]
    private void SpawnPlayerServerRpc(ulong sender)
    {
        GameObject ship = Instantiate(playerShipPrefab);

        NetworkObject instanceNetworkObject = ship.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        instanceNetworkObject.ChangeOwnership(sender);

        PopulateOnSpawnClientRPC(instanceNetworkObject.NetworkObjectId, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { sender }
            }
        });
    }

    [ClientRpc]
    private void PopulateOnSpawnClientRPC(ulong objectId, ClientRpcParams rpcParams = default)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
        {
            Debug.LogWarning($"[Client] Could not find object with id {objectId}");
            return;
        }

        GameObject go = netObj.gameObject;

        GravityImpact = go.GetComponent<GravityImpact>();
        PlayerController = go.GetComponent<PlayerController>();
    }
}
