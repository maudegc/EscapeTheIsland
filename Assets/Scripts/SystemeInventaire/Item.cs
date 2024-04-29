using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Item : NetworkBehaviour
{
    private readonly NetworkVariableInt _nbrCordeNetwork = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    private readonly NetworkVariableInt _nbrBoisNetwork = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    private readonly NetworkVariableInt _nbrTissuNetwork = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });
    private readonly NetworkVariableInt _nbrRameNetwork = new NetworkVariableInt(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariable<GameObject> _inventaireNetwork = new NetworkVariable<GameObject>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    [SerializeField] public string type;
    [SerializeField] private int quantity;
    [SerializeField] private string nom;

    private Inventory _inventory;
    private ItemDictionary _itemDictionary;
    private ItemType _itemType;

    private GameObject inventaireImportant;
    private GameObject cordeObject;
    private GameObject boisObject;
    private GameObject rameObject;
    private GameObject tissuObject;
   
    private static List<String> listeObjetsInventaire = new List<String>();
   
    public Image imageBois;
    public Image imageCorde;
    public Image imageRame;
    public Image imageTissu;

    static int  nbrElements;
    
    static int nbrTissu;
    static int nbrBois;
    static int nbrRame;
    static int nbrCorde;
 
    private void Awake()
    {
        //_inventaireNetwork.Value = GameObject.FindWithTag("InventoryObject");

        inventaireImportant = GameObject.Find("InventaireObjet");
        _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        _itemDictionary = GameObject.FindWithTag("ItemDictionary").GetComponent<ItemDictionary>();     

        boisObject = GameObject.FindWithTag("plancheObjet");
        rameObject = GameObject.FindWithTag("rameObjet");
        tissuObject = GameObject.FindWithTag("tissuObjet");
        cordeObject = GameObject.FindWithTag("cordeObjet");
        
        imageRame = GameObject.Find("ImageRame").GetComponent<Image>();
        imageCorde = GameObject.Find("ImageCorde").GetComponent<Image>();
        imageBois = GameObject.Find("ImagePlancheBois").GetComponent<Image>();
        imageTissu = GameObject.Find("ImageTissu").GetComponent<Image>();
    }

    void Start()
    {
        _itemType = _itemDictionary.GetItemType(type);
        /*int nbrCorde = 0;
        int nbrTissu = 0;
        int nbrBois = 0;
        int nbrRame = 0;*/
    }

    public static List<string> GetListeObjetsInventaire()
    {
        return listeObjetsInventaire;
    }

    [ServerRpc (RequireOwnership = false)]
    void SubmitDestroyItemRequestServerRpc(ServerRpcParams param = default)
    {
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitNbrCordeRequestServerRpc(ServerRpcParams param = default)
    {
        nbrCorde++;
        _nbrCordeNetwork.Value = nbrCorde;

        //_inventaireNetwork.Value = GameObject.FindWithTag("InventoryObject");
        //_inventaireNetwork.Value.transform.Find("InventaireObjet").transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = _nbrCordeNetwork.Value.ToString();

        inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = _nbrCordeNetwork.Value.ToString();
        imageCorde.color = new Color32(0, 255, 0, 255);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitNbrBoisRequestServerRpc(ServerRpcParams param = default)
    {
        nbrBois++;
        _nbrBoisNetwork.Value = nbrBois;

        inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = _nbrBoisNetwork.Value.ToString();

        /*_texteBoisNetwork.Value = GameObject.Find("textObjetBois").GetComponent<Text>();
        _texteBoisNetwork.Value.text = _nbrBoisNetwork.Value.ToString();*/
        //_inventaireNetwork.Value.transform.Find("InventaireObjet").transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = _nbrBoisNetwork.Value.ToString();


        if (_nbrBoisNetwork.Value == 5)
        {
            imageBois.color = new Color32(0, 255, 0, 255);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitNbrRameRequestServerRpc(ServerRpcParams param = default)
    {
        nbrRame++;
        _nbrRameNetwork.Value = nbrRame;

        inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text = _nbrRameNetwork.Value.ToString();
        imageRame.color = new Color32(0, 255, 0, 255);
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitNbrTissuRequestServerRpc(ServerRpcParams param = default)
    {
        nbrTissu++;
        _nbrTissuNetwork.Value = nbrTissu;

        inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text = _nbrTissuNetwork.Value.ToString();
        if (_nbrTissuNetwork.Value == 5)
        {
            imageTissu.color = new Color32(0, 255, 0, 255);
        }
    }

    private void Update()
    {
      
    }

    public void Pickup()
    {
        listeObjetsInventaire.Add(type);

        if (gameObject == cordeObject)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                nbrCorde++;
                _nbrCordeNetwork.Value = nbrCorde;

                
               // _inventaireNetwork.Value.transform.Find("InventaireObjet").transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = nbrBois.ToString();

                inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = _nbrCordeNetwork.Value.ToString();
                imageCorde.color = new Color32(0, 255, 0, 255);
            }
            else
            {
                SubmitNbrCordeRequestServerRpc();
            }
        }
        else if (gameObject == boisObject)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                nbrBois++;
                _nbrBoisNetwork.Value = nbrBois;

                inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = _nbrBoisNetwork.Value.ToString();

                //_inventaireNetwork.Value.transform.Find("InventaireObjet").transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = nbrBois.ToString();

                if (_nbrBoisNetwork.Value == 5)
                {
                    imageBois.color = new Color32(0, 255, 0, 255);
                }
            }
            else
            {           
                SubmitNbrBoisRequestServerRpc();
            }
          
        }
        else if (gameObject == rameObject)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                nbrRame++;
                _nbrRameNetwork.Value = nbrRame;

                inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text = nbrRame.ToString();
                Debug.Log("Hello rame??");
                imageRame.color = new Color32(0, 255, 0, 255);
                Debug.Log("Non");
            }
            else
            {
                SubmitNbrRameRequestServerRpc();
            }
        }
        else if (gameObject == tissuObject)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                nbrTissu++;
                _nbrTissuNetwork.Value = nbrTissu;

                inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text = _nbrTissuNetwork.Value.ToString();
                if (_nbrTissuNetwork.Value == 5)
                {
                    imageTissu.color = new Color32(0, 255, 0, 255);
                }
            }
            else
            {
                SubmitNbrTissuRequestServerRpc();
            }       
        }
        
        int remaining = _inventory.AddItemStack(new ItemStack(_itemType, quantity));

        if (remaining == 0)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Destroy(gameObject);
            }
            else
            {
                SubmitDestroyItemRequestServerRpc();
            }
        }
        else
        {
            quantity = remaining;
        }
    }  
}
