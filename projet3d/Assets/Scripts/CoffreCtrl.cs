using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe permettant d'animer le coffre
/// </summary>
public class CoffreCtrl : NetworkBehaviour
{
    private NetworkAnimator _animNetwork;
    private Animator _anim;
    private GameObject _coffre;

    private bool _coffreOuvert = false;

    public GameObject _corde;

    private void Awake()
    {
        _coffre = GameObject.FindWithTag("Chest");
    }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitAnimationCoffreServerRpc(ServerRpcParams rpcParams = default)
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("ouvrir", true);
        GameObject corde = Instantiate(_corde);
        corde.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }

    public void OuvrirCoffre()
    {
        if (_coffreOuvert)
        {
            return;
        }
        if (NetworkManager.Singleton.IsServer)
        {
            _anim = GetComponent<Animator>();
            _anim.SetBool("ouvrir", true);
            GameObject corde = Instantiate(_corde);
            corde.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }
        else 
        {
            SubmitAnimationCoffreServerRpc();
        }
        ((Outline)_coffre.GetComponent<Outline>()).enabled = false;
        _coffreOuvert = true;
    }
}