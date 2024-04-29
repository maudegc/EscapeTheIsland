using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetecterObjetRammasse : NetworkBehaviour
{
    [SerializeField] GameObject _pickUpSlot;
    [SerializeField] private UnityEvent _activer;
    [SerializeField] GameObject _objetADetecter;

    private GameObject _cle;
    private GameObject _hache;

    private PersonnageCtrl _perso;

    private NetworkAnimator _animHache;
    private Animator _animatorHache;

    void Start()
    {
        _perso = GetComponent<PersonnageCtrl>();
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitActiveInvokeServerRpc(ServerRpcParams rpcParams = default)
    {
        _activer.Invoke();
    }
    public void Detecter()
    {
        if (_perso == null)
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
                if (player.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    _perso = player.GetComponent<PersonnageCtrl>();
                }
        }
        if (_perso._slot.transform.childCount != 0)
        {
            if (_perso._slot.transform.GetChild(0).tag == _objetADetecter.tag)
            {
                if (_perso._slot.transform.GetChild(0).tag == "Axe")
                {
                    _hache = _perso._slot.transform.GetChild(0).gameObject;
                    if (_hache.activeSelf == false)
                    {
                        return;
                    }
                }
                if (_perso._slot.transform.GetChild(0).tag == "key")
                {
                    _cle = _perso._slot.transform.GetChild(0).gameObject;
                    if (_cle.activeSelf == false)
                    {
                        return;
                    }
                }
                if (NetworkManager.Singleton.IsServer)
                {
                    _activer.Invoke();
                }
                else
                {
                    SubmitActiveInvokeServerRpc();
                }
                if (_perso._slot.transform.GetChild(0).tag == "Axe")
                {
                    _hache = _perso._slot.transform.GetChild(0).gameObject;
                    _animHache = _hache.GetComponent<NetworkAnimator>();
                    if (_hache.activeSelf == true)
                    {
                        _animHache.Animator.SetTrigger("couper");
                    }
                }
            }
        }
    } 
}
