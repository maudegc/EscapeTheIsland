using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiCtrl : NetworkBehaviour
{
    [SerializeField] public float distanceCourir = 10f;
    [SerializeField] private LayerMask layerPerso;

    private NavMeshAgent _agent;
    private GameObject _joueur;
    private Animator anim;
    private NetworkAnimator _netAnim;

    bool courir = false;
    
   
    private void Start()
    {
       /* foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                _joueur = player.GetComponent<GameObject>();
            }
        }*/
        _joueur = GameObject.FindWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        DetecterJoueurCourir();
        anim.SetBool("courir", courir);

        if (courir)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                
                SuivreJoueur();
            }
            else
            { 
                SubmitSuivreRequestServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitSuivreRequestServerRpc(ServerRpcParams param = default)
    {
        SuivreJoueur();
    }

    public void SuivreJoueur()
    {
        if (_joueur == null)
        {
            _joueur = GameObject.FindWithTag("Player");
            /*foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    _joueur = player.GetComponent<GameObject>();
                }
            }*/
        }
       _agent.SetDestination(_joueur.transform.position);
    }

    public void DetecterJoueurCourir()
    {
        float distanceTotal = distanceCourir + 0.2f;

        bool raycastHit = Physics.Raycast(transform.position, Vector3.forward,
            distanceTotal, layerPerso);
        
        Color rayColor;
        rayColor = raycastHit ? Color.green : Color.red;

        Debug.DrawRay(transform.position, Vector3.forward*distanceTotal, rayColor);

        if (raycastHit)
        {
            courir = true;
        }
    }
    
     public void Attaquer()
     {
        anim.SetTrigger("attaquer");
     }

     public void Mourir()
     {
        anim.SetBool("mourir", true);
        Destroy(gameObject, 4);
     }

     public void Damage()
     {
        anim.SetTrigger("damage");
     }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") {
            Attaquer();         
        }
    }
}
