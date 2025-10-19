
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GravityImpact : NetworkBehaviour
{
    public static List<GravityImpact> gravityImpacts = new List<GravityImpact>();
    public NetworkVariable<float> gravityForce = new NetworkVariable<float>(1f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public float minGravity = 1f;
    public float maxGravity = 10f;
    public bool isImpacted = true;

    private SpaceshipController spaceshipController;
    private Animator animator;

    private void Awake()
    {
        spaceshipController = GetComponent<SpaceshipController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gravityImpacts.Add(this);
    }

    private void OnDisable()
    {
        gravityImpacts.Remove(this);
    }

    private void FixedUpdate()
    {
        if (!isImpacted) return;
        foreach (GravityImpact GI in gravityImpacts)
        {
            if (GI == this) continue;

            Vector3 direction = GI.transform.position - transform.position;
            Vector3 force = direction.normalized * GI.gravityForce.Value;
            GetComponent<Rigidbody2D>().AddForce(force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Death"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if(spaceshipController != null)
            spaceshipController.isAlive = false;
        animator.SetBool("explode", true);
        if (spaceshipController.IsOwner)
        {
            LocalPlayerManager.MyLocalPlayerManager.PlayerController = null;
            LocalPlayerManager.MyLocalPlayerManager.GravityImpact = null;
        }
    }

    public void Die()
    {
        if (IsServer)
        {
            NetworkObject no = gameObject.GetComponent<NetworkObject>();
            NotifyOwnerOfDeathClientRPC(new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { no.OwnerClientId }
                }
            }); 
            no.Despawn(true);
        }
    }
    
    [ClientRpc]
    private void NotifyOwnerOfDeathClientRPC(ClientRpcParams rpcParams = default)
    {
        UIManager.Instance.toggleRespawnButton();
    }

    public void ChangeGravity(float change)
    {
        if (!IsOwner) return;
        gravityForce.Value += change;
        if (gravityForce.Value < minGravity) gravityForce.Value = minGravity;
        else if (gravityForce.Value > maxGravity) gravityForce.Value = maxGravity;
    }
}
