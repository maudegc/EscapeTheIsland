using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : NetworkBehaviour
{
    [SerializeField] GameObject _drop;
    Transform positionInit;
    bool appFerme = false;

    void Start()
    {
        positionInit = gameObject.transform;
    }

    [ServerRpc]
    void SubmitDropRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Instantiate(_drop, positionInit.position + Vector3.one, Quaternion.identity).GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }

    private void OnDestroy()
    {
        if (!appFerme)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Instantiate(_drop, positionInit.position + Vector3.one, Quaternion.identity).GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }
            /*else 
            {
                SubmitDropRequestServerRpc();
            }*/
        } 
    }
    private void OnApplicationQuit()
    {
        appFerme = true;
    }
}