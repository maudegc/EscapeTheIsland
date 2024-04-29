using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using MLAPI.Spawning;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe correspondant au personnage
/// 
/// Contient les informations sur le personnage (Vitesse, Santé)
/// Permet de contrôler le personnage (Bouger, RotationY, RotationX, Sauter)
/// </summary>
public class PersonnageCtrl : NetworkBehaviour
{
    private readonly NetworkVariableVector3 _positionNetwork = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariableBool _hacheStateNetwork = new NetworkVariableBool(new NetworkVariableSettings 
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariableBool _cleStateNetwork = new NetworkVariableBool(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariable<GameObject> _hacheRamasseNetwork = new NetworkVariable<GameObject>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariable<GameObject> _cleRamasseNetwork = new NetworkVariable<GameObject>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    [SerializeField] private float _intervalleRotationX = 170;
    [SerializeField] private float vitesse = 7f;
    [SerializeField] private float distanceRamassage = 5;
    [SerializeField] private LayerMask layerSol;
    [SerializeField] private LayerMask layerInteractible;
    [SerializeField] public GameObject _slot;

    private Camera _mainCamera;
    private Rigidbody _rb;
    private Rigidbody _rbObjetRammase;
    private CapsuleCollider _collider;
    private GameObject _objetRamasse = null;
    private GameObject _interactibleRegarde;
   
    private GameObject _hacheEquiped;

    private Vector3 _positionInitialeCamera;
    private Vector3 directionMouvement = Vector3.zero;

    private float _pivot = 180;
    private float _minRotationX;
    private float _maxRotationX;
    private float _sante = 100;

    private bool _hacheActive = false;
    private bool _cleActive = false;

    private Canvas _inventaire;
    private Canvas _inventaireObjet;

    public ProgressBar _pb;

    public GameObject _hache;
    public GameObject _cle;
    public GameObject _chest;
    public GameObject _corde;
    public GameObject _canvasObjetInventaire;

   /* [ServerRpc(RequireOwnership = false)]
    private void SubmitSpawnObjetServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject canvasInventaire = Instantiate(_canvasObjetInventaire);
        canvasInventaire.transform.parent = transform;
        canvasInventaire.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }

    public override void NetworkStart()
    {
        //base.NetworkStart();

        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {

            GameObject canvasInventaire = Instantiate(_canvasObjetInventaire);
            canvasInventaire.transform.parent = transform;
            canvasInventaire.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }
    }*/

    private void Awake()
    {
        _mainCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        _positionNetwork.Value = transform.position;

        if (!IsLocalPlayer)
        {
            Destroy(_mainCamera.gameObject);
            return;
        }

        _pb = GameObject.FindWithTag("HealthBar").GetComponent<ProgressBar>();
        _inventaire = GameObject.FindWithTag("Inventory").GetComponent<Canvas>();
        _inventaireObjet = GameObject.FindWithTag("InventoryObject").GetComponent<Canvas>();

        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();

        _pb.BarValue = 100;
        _minRotationX = _pivot - (_intervalleRotationX / 2f);
        _maxRotationX = _pivot + (_intervalleRotationX / 2f);
        _mainCamera = Camera.main;

        _positionInitialeCamera = _mainCamera.transform.position;
    }

    [ServerRpc]
    private void SubmitPositionRequestServerRpc(Vector3 currentPosition, ServerRpcParams rpcParams = default)
    {      
        _positionNetwork.Value = currentPosition;
    }

    [ServerRpc]
    private void SubmitHacheStateRequestServerRpc(bool state, ServerRpcParams rpcParams = default)
    {
        if (_slot.transform.GetChild(0).tag == "Axe")
        {

            _slot.transform.GetChild(0).gameObject.SetActive(state);
        }
        else if (transform.Find("Hache").tag == "Axe")
        {
            transform.Find("Hache").gameObject.SetActive(state);
        }
        else
        {
            _hacheStateNetwork.Value = state;
        }
    }

    [ServerRpc]
    private void SubmitStateRequestServerRpc(bool state, ServerRpcParams rpcParams = default)
    {
        _hacheStateNetwork.Value = state;
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                _positionNetwork.Value = transform.position;
            }
            else
            {
                SubmitPositionRequestServerRpc(transform.position);
              
            }
        }
        else
        {
            transform.position = _positionNetwork.Value;
        }

       // if (NetworkManager.Singleton.IsServer)
       // {

            /* if (_slot.transform.GetChild(0).tag == "Axe")
             {
                 _slot.transform.GetChild(0).gameObject.SetActive(_hacheStateNetwork.Value);
             }
             else if (transform.Find("Hache").tag == "Axe")
             {
                 transform.Find("Hache").gameObject.SetActive(_hacheStateNetwork.Value);
             }*/
           // _hacheStateNetwork.Value = _hacheActive;
      //  }
       // else 
      //  {
            //SubmitHacheStateRequestServerRpc(_hacheActive);
          //  SubmitStateRequestServerRpc(_hacheActive);
      //  }
    }

    void FixedUpdate()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        if (RaycastUtil.DebugMode)
        {
            EstAuSol();
        }

        GameObject interactible = GetInteractible();
        if (interactible != _interactibleRegarde)
        {
            if (_interactibleRegarde != null)
            {
                _interactibleRegarde.GetComponent<Interactible>().Deselectionner();
            }

            if (interactible != null)
            {
                interactible.GetComponent<Interactible>().Selectionner();
            }

            _interactibleRegarde = interactible;
        }

        if (Input.GetKeyDown("i"))
        {
            if (_inventaireObjet.enabled == true)
            {
                _inventaireObjet.enabled = false;
            }
            else if (_inventaireObjet.enabled == false)
            {
                _inventaireObjet.enabled = true;
            }
        }
    }

    public float GetSante()
    {
        return _sante;
    }

    /// <summary>
    /// Permet de regarder vers la droite et la gauche
    /// </summary>
    /// <param name="angle">
    /// 
    /// </param>
    public void RotationY(float angle)
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        transform.Rotate(0, angle, 0);
    }

    /// <summary>
    /// Permet de regarder vers le haut et le bas
    /// </summary>
    /// <param name="angle">
    /// 
    /// </param>
    public void RotationX(float angle)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        _mainCamera.transform.Rotate(angle, 0, 0);
        float rotationY = _mainCamera.transform.eulerAngles.x;
        rotationY += _pivot;
        rotationY %= 360;

        if (rotationY < _minRotationX || rotationY > _maxRotationX)
        {
            _mainCamera.transform.Rotate(-angle, 0, 0);
        }
    }

    /// <summary>
    /// Permet de bouger le personnage
    /// </summary>
    /// <param name="horizontal">
    /// 
    /// </param>
    /// <param name="vertical">
    /// 
    /// </param>
    public void Bouger(float horizontal, float vertical)
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        if (EstAuSol()) {
            directionMouvement.x = horizontal;
            directionMouvement.y = vertical;
            Vector3 nouvelleDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
            nouvelleDirection *= vitesse;
            _rb.velocity = nouvelleDirection;
        }
    }

    /// <summary>
    /// Le personnage perd de la santé quand il se fait frapper par l'ennemi
    /// </summary>
    /// <param name="other">
    /// L'ennemi qui touchera le personnage
    /// </param>
    public void OnCollisionEnter(Collision other)
    {
        EnnemiCtrl ennemi = other.gameObject.GetComponent<EnnemiCtrl>();
        if (ennemi)
        {
            _pb.BarValue--;
            _sante = _pb.BarValue;
        }
    }

    /// <summary>
    /// Permet de définir si le personnage et au sol
    /// </summary>
    /// <returns>
    /// true si le personnage touche au sol
    /// false si le personnage ne touche pas le sol
    /// </returns>
    private bool EstAuSol()
    {
        Bounds bounds = _collider.bounds;
        return RaycastUtil.TesterCollision(bounds.center, Vector2.down, bounds.extents.y, layerSol);
    }

    public GameObject GetInteractible()
    {
        if (!IsLocalPlayer)
        {
            return null;
        }
        var cameraTransform = _mainCamera.transform;
        return RaycastUtil.TesterCollisionObjet(cameraTransform.position,
            cameraTransform.forward, distanceRamassage, layerInteractible);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitDestroHacheRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Destroy(_hacheRamasseNetwork.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitDestroyCleRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Destroy(_cleRamasseNetwork.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitActiveHacheRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        _hacheStateNetwork.Value = true;
        
    }

    /// <summary>
    /// Permet de ramasser un objet
    /// </summary>
    /// <param name="objet">
    /// L'objet à ramasser
    /// </param>
    public void Ramasser(GameObject objet)
    {
        if (_objetRamasse != null)
        {
            return;
        }
     
        _objetRamasse = objet;
        
        if (!_hacheActive)
        {
            _hacheRamasseNetwork.Value = _objetRamasse;
            _hacheStateNetwork.Value = true;

            GameObject hacheCible = null;
            
            foreach (var hache in GameObject.FindGameObjectsWithTag("Axe"))
            {
                hacheCible = hache;

            if (_objetRamasse == hacheCible)
            {
                    GameObject hacheE = transform.Find("Hache").gameObject;
                    hacheE.transform.parent = _slot.transform;
                    hacheE.GetComponent<Animator>().enabled = true;
                    _hacheEquiped = hacheE;
                 
                     if (NetworkManager.Singleton.IsServer)
                     {
                        _hacheRamasseNetwork.Value = _objetRamasse;
                         Destroy(_objetRamasse);
                     }
                     else
                     {
                        SubmitDestroHacheRequestServerRpc();
                     }                 

                    _hacheActive = true;
                    

                    _objetRamasse = _hacheEquiped;

                    if (NetworkManager.Singleton.IsServer)
                    {
                        _hacheStateNetwork.Value = true; ;
                        //_slot.transform.Find("Hache").gameObject.SetActive(_hacheStateNetwork.Value);
                    }
                    else
                    {
                        SubmitActiveHacheRequestServerRpc();
                    }
                    _slot.transform.Find("Hache").gameObject.SetActive(_hacheStateNetwork.Value);
                }
            }
        }
                      
        if (_objetRamasse == GameObject.Find("Cle(Clone)"))
        {
            if (!_cleActive)
            {
                _cleRamasseNetwork.Value = _objetRamasse;

                GameObject cle = transform.Find("Cle").gameObject;
                cle.transform.parent = _slot.transform;

                Rigidbody rbCle = cle.GetComponent<Rigidbody>();
                rbCle.useGravity = false;
                rbCle.isKinematic = true;


                if (NetworkManager.Singleton.IsServer)
                {
                    _cleRamasseNetwork.Value = _objetRamasse;
                    Destroy(_objetRamasse);
                }
                else
                {
                    SubmitDestroyCleRequestServerRpc();
                }
               
                _cleActive = true;
                _cleStateNetwork.Value = _cleActive;

                _objetRamasse = cle;
                _slot.transform.Find("Cle").gameObject.SetActive(_cleStateNetwork.Value);
            }
        }      
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitLacherHacheRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject hacheSpawn = Instantiate(_hache, _positionNetwork.Value + transform.forward * 5, Quaternion.identity);
        Rigidbody rbHache = hacheSpawn.GetComponent<Rigidbody>();
        rbHache.useGravity = true;
        rbHache.isKinematic = false;
        hacheSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitLacherCleRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject cleSpawn = Instantiate(_cle, _positionNetwork.Value + transform.forward * 3, Quaternion.identity);
        Rigidbody rbCle = cleSpawn.GetComponent<Rigidbody>();
        rbCle.useGravity = true;
        rbCle.isKinematic = false;
        cleSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
    }

    /// <summary>
    /// Permet de lacher l'objet que l'on tient
    /// </summary>
    /// <returns>
    /// true si on a laché l'objet
    /// false si on n'a pas lacher l'objet
    /// </returns>
    public void Lacher()
    {
        if (_objetRamasse == null)
        {
            return;
        }

        if (_hacheActive)
        {
            GameObject hacheE = _slot.transform.GetChild(0).gameObject; 
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject hacheSpawn = Instantiate(_hache, transform.position + transform.forward * 5, Quaternion.identity);
                Rigidbody rbHache = hacheSpawn.GetComponent<Rigidbody>();
                rbHache.useGravity = true;
                rbHache.isKinematic = false;
                hacheSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }
            else 
            {
                SubmitLacherHacheRequestServerRpc();
            }        
            hacheE.transform.parent = transform;
            _hacheActive = false;
            _hacheStateNetwork.Value = _hacheActive;         
            transform.Find("Hache").gameObject.SetActive(_hacheStateNetwork.Value);
            _objetRamasse = null;
        }

        if (_cleActive)
        {
            GameObject cle = _slot.transform.GetChild(0).gameObject;
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject cleSpawn = Instantiate(_cle, transform.position + transform.forward * 3, Quaternion.identity);
                Rigidbody rbCle = cleSpawn.GetComponent<Rigidbody>();
                rbCle.useGravity = true;
                rbCle.isKinematic = false;
                cleSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }
            else
            {
                SubmitLacherCleRequestServerRpc();
            }
            cle.transform.parent = transform;
            //cle.SetActive(false);
            _cleActive = false;
            _cleStateNetwork.Value = _cleActive;
            transform.Find("Cle").gameObject.SetActive(_cleStateNetwork.Value);
            _objetRamasse = null;
        }           
    }

    /// <summary>
    /// Permet d'activer le interactable que l'on regarde
    /// </summary>
    public void Activer()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        if (_interactibleRegarde != null)
        {
            Interactible interactible = _interactibleRegarde.GetComponent<Interactible>();
            interactible.Activer();
        }   
    }
}
