using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SpaceshipController
{
    void Update()
    {
        if (!isAlive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (IsOwner)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0));
            RotateServerRPC(pos);
        }
        Move();
    }

    private void FixedUpdate()
    {
        if (!isAlive || !IsOwner) return;
        Move();
    }

    private void Move()
    {
        if (!isAlive || !IsOwner) return;
        MoveServerRPC();
    }

    [ServerRpc(RequireOwnership = true)]
    private void MoveServerRPC()
    {
        rb.AddForce(transform.up * acceleration * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    [ServerRpc(RequireOwnership = true)]
    private void RotateServerRPC(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public IEnumerator DrawRoutine()
    {
        WaitForSeconds interval = new WaitForSeconds(0.1f); // 10× per second
        while (InputManager.Instance.holdingM1)
        {
            if(InputManager.Instance.currentGravLineObject != null)
                TryDrawOnce();
            yield return interval;
        }
    }

    public void TryDrawOnce()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        TryDrawOnceServerRPC(this.OwnerClientId, worldPos);
    }

    [ServerRpc(RequireOwnership = true)]
    private void TryDrawOnceServerRPC(ulong sender, Vector3 worldPos)
    {
        if (InputManager.Instance.currentGravLineObject == null)
            return;

        worldPos.z = 0;

        GameObject go = Instantiate(InputManager.Instance.DrawnGravObjectPrefab, worldPos, Quaternion.identity, InputManager.Instance.currentGravLineObject[sender].transform);
        NetworkObject no = go.GetComponent<NetworkObject>();
        no.Spawn();
        no.ChangeOwnership(sender);

        DrawnGravLineController dglc = InputManager.Instance.currentGravLineObject[sender].GetComponent<DrawnGravLineController>();
        dglc.drawnGravObjects.Value.Add(go.transform.position);
        go.GetComponent<DrawnGravObjectController>().parent = dglc;

        PopulateLineRendererClientRPC(no.NetworkObjectId, sender);
    }

    [ClientRpc]
    private void PopulateLineRendererClientRPC(ulong objectId, ulong owner)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
        {
            Debug.LogWarning($"[Client] Could not find object with id {objectId}");
            return;
        }

        if(InputManager.Instance.currentGravLineObject.ContainsKey(owner))
            InputManager.Instance.currentGravLineObject[owner].GetComponent<DrawnGravLineController>().drawnGravObjects.Value.Add(netObj.gameObject.transform.position);
    }

    public void StartNewGravLine()
    {
        StartNewGravLineServerRPC(this.OwnerClientId);
    }

    [ServerRpc(RequireOwnership = true)]
    private void StartNewGravLineServerRPC(ulong sender)
    {
        InputManager.Instance.currentGravLineObject[sender] = Instantiate(InputManager.Instance.DrawnGravLinePrefab);

        NetworkObject instanceNetworkObject = InputManager.Instance.currentGravLineObject[sender].GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
        instanceNetworkObject.ChangeOwnership(sender);

        StartNewGravLineClientRPC(instanceNetworkObject.NetworkObjectId);
    }

    [ClientRpc]
    private void StartNewGravLineClientRPC(ulong objectId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
        {
            Debug.LogWarning($"[Client] Could not find object with id {objectId}");
            return;
        }

        InputManager.Instance.currentGravLineObject[this.OwnerClientId] = netObj.gameObject;
    }
}
