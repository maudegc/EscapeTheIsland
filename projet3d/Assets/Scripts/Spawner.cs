using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    public GameObject _hache;
    public GameObject _corde;
    public GameObject _chest;
    public GameObject _paddle;
    public GameObject _canvasObjetInventaire;

    [ServerRpc(RequireOwnership = false)]
    private void SubmitSpawnHacheServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject hache2 = Instantiate(_hache);
        hache2.transform.position = new Vector3(49, 16, 9);
        hache2.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);

       /* GameObject canvasInventaire = Instantiate(_canvasObjetInventaire);
        canvasInventaire.transform.parent = transform;
        canvasInventaire.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);*/
    }

    public override void NetworkStart()
    {
        //base.NetworkStart();

        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            GameObject chest = Instantiate(_chest);
            chest.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
  
            GameObject paddle = Instantiate(_paddle);
            paddle.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);

            GameObject hache1 = Instantiate(_hache);
            hache1.transform.position = new Vector3(51, 16, 9);
            hache1.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        
           /* GameObject canvasInventaire = Instantiate(_canvasObjetInventaire);
            canvasInventaire.transform.parent = transform;
            canvasInventaire.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);*/
        }
        else 
        {
            SubmitSpawnHacheServerRpc();
        }
    }
}
