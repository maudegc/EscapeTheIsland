using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Spawning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant de ramasser l'objet en appelant la méthode Ramasser de PersonnageCtrl
/// </summary>
public class Ramasser : NetworkBehaviour
{
    private readonly NetworkVariableVector3 _positionNetwork = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    
    private PersonnageCtrl _persoCtrl;
    
    /// <summary>    
    /// Appel la fonction ramasser    
    /// </summary>       
    public void Pickup()
    {
        if (_persoCtrl != null)
        {
            _persoCtrl.Ramasser(gameObject);
        }
        else 
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    _persoCtrl = player.GetComponent<PersonnageCtrl>();
                }
            }
        }
    }
}
