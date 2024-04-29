using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI.Messaging;
using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.NetworkVariable;

public class Poser : NetworkBehaviour
{

    private readonly NetworkVariableBool _imageCrochetStateNetwork = new NetworkVariableBool(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private readonly NetworkVariable<GameObject> _objetNetwork = new NetworkVariable<GameObject>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.Everyone,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public GameObject humanRenderMesh;
    public Renderer ren;
    public Material[] mat;
    private JeuCtrl jeu;
    private string type;
    private Item unObjet;
    static int materiauxRaft;

    GameObject inventaireImportant;
    void Start()
    {
        ren = gameObject.GetComponent<Renderer>();
        mat = ren.materials;

       inventaireImportant = GameObject.Find("InventaireObjet");

        _imageCrochetStateNetwork.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitPoseRequestServerRpc()
    {
        List<string> liste = Item.GetListeObjetsInventaire();
        string typeObjet = gameObject.GetComponent<Item>().type;
        Image imageObjet = gameObject.GetComponent<Item>().imageBois;
       // GameObject inventaireImportant = GameObject.Find("InventaireObjet");

        Color c = ren.material.color;
        c.a = 1f;

        for (int i = 0; i < Item.GetListeObjetsInventaire().Count; i++)
        {
            string objetInventaire = liste[i].ToString();
            print(objetInventaire);
            print(typeObjet);

            if (objetInventaire.Equals("bois") && typeObjet.Equals("bois"))
            {
                if (inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text == "5")
                {
                    gameObject.GetComponent<Renderer>().material.color = c;
                    print(materiauxRaft);
                    inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                    materiauxRaft++;
                    inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = "0";
                }
            }
            else if (objetInventaire.Equals("corde") && typeObjet.Equals("corde"))
            {
                if (inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text == "1")
                {              
                    gameObject.GetComponent<Renderer>().material.color = c;
                    inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                    materiauxRaft++;
                    inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = "0";
                }
            }
            else if (objetInventaire.Equals("tissu") && typeObjet.Equals("tissu"))
            {
                if (inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text == "5")
                {
                    gameObject.GetComponent<Renderer>().material.color = c;
                    inventaireImportant.transform.Find("BackgroundObjet").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                    materiauxRaft++;
                    inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text = "0";
                }
            }
            else if (objetInventaire.Equals("rame") && typeObjet.Equals("rame"))
            {
                if (inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text == "1")
                {

                    gameObject.GetComponent<Renderer>().material.color = c;
                    inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                    materiauxRaft++;
                    inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text = "0";
                }
            }
            if (materiauxRaft == 4)
            {
                //NetworkSceneManager.SwitchScene("Gagner");
                SceneManager.LoadScene("Gagner");
            }
        }
    }

    public void Pose()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            List<string> liste = Item.GetListeObjetsInventaire();
            string typeObjet = gameObject.GetComponent<Item>().type;
            Image imageObjet = gameObject.GetComponent<Item>().imageBois;
           // GameObject inventaireImportant = GameObject.Find("InventaireObjet");

            Color c = ren.material.color;
            c.a = 1f;

            for (int i = 0; i < Item.GetListeObjetsInventaire().Count; i++)
            {
                string objetInventaire = liste[i].ToString();
                print(objetInventaire);
                print(typeObjet);

                if (objetInventaire.Equals("bois") && typeObjet.Equals("bois"))
                {
                    if (inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text == "5")
                    {
                        gameObject.GetComponent<Renderer>().material.color = c;
                        print(materiauxRaft);
                        inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                        materiauxRaft++;
                        inventaireImportant.transform.Find("BackgroundObjet1").transform.Find("textObjetBois").GetComponent<Text>().text = "0";
                    }

                }
                else if (objetInventaire.Equals("corde") && typeObjet.Equals("corde"))
                {
                    if (inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text == "1")
                    {               
                        gameObject.GetComponent<Renderer>().material.color = c;
                        inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                        materiauxRaft++;
                        inventaireImportant.transform.Find("BackgroundObjet2").transform.Find("textObjetCorde").GetComponent<Text>().text = "0";
                    }
                }
                else if (objetInventaire.Equals("tissu") && typeObjet.Equals("tissu"))
                {
                    if (inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text == "5")
                    {
                        gameObject.GetComponent<Renderer>().material.color = c;
                        inventaireImportant.transform.Find("BackgroundObjet").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                        materiauxRaft++;
                        inventaireImportant.transform.Find("BackgroundObjet").transform.Find("textObjetTissu").GetComponent<Text>().text = "0";
                    }
                }
                else if (objetInventaire.Equals("rame") && typeObjet.Equals("rame"))
                {
                    if (inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text == "1")
                    {

                        gameObject.GetComponent<Renderer>().material.color = c;
                        inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("crochet").GetComponent<Canvas>().enabled = _imageCrochetStateNetwork.Value;
                        materiauxRaft++;
                        inventaireImportant.transform.Find("BackgroundObjet3").transform.Find("textObjetRame").GetComponent<Text>().text = "0";
                    }
                }
                if (materiauxRaft == 4)
                {

                    //NetworkSceneManager.SwitchScene("Gagner");
                    SceneManager.LoadScene("Gagner");
                }

            }
        }
        else
        {
            SubmitPoseRequestServerRpc();
        }
    } 
}
