using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe qui représente le jeu
/// 
/// Contient les méthodes appelées pour faire fonctionner le jeu
/// </summary>
public class JeuCtrl : NetworkBehaviour
{
    [SerializeField] public float sensibiliteSouris = 1;

    private PersonnageCtrl _persoCtrl;
    private Pause pause = new Pause();

    private bool _estActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        _persoCtrl = transform.parent.GetComponent<PersonnageCtrl>();

    }

    void Start()
    {
        if (!_persoCtrl.IsLocalPlayer)
        {
            return;
        }
        RaycastUtil.DebugMode = true;
        RaycastUtil.VerboseMode = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    
        #if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        #else
        Debug.unityLogger.logEnabled = false;
        #endif

    }

    [ServerRpc]
    void SubmitPauseRequestServerRpc(ServerRpcParams param = default)
    {
        pause.PausePartie();
        ScenePause();
    }

    // Update is called once per frame
    void Update()
    {
        if (_persoCtrl.IsLocalPlayer)
        {
            if (Input.GetAxis("Cancel") != 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            float mouseX = Input.GetAxis("Mouse X") * sensibiliteSouris;
            if (mouseX != 0)
            {
                _persoCtrl.RotationY(mouseX);
            }
            float mouseY = Input.GetAxis("Mouse Y") * sensibiliteSouris;
            if (mouseY != 0)
            {
                _persoCtrl.RotationX(-mouseY);
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                _persoCtrl.Bouger(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    pause.PausePartie();
                    ScenePause();
                }
                else
                {
                    SubmitPauseRequestServerRpc();
                }
            }

            if (_persoCtrl.GetSante() <= 0)
            {
                Destroy(_persoCtrl);
                ScenePerdante();
            }
        }
    }

    public void ScenePause()
    { 
        gameObject.GetComponent<ChangeScene>().LoadScene("Pause");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ScenePerdante()
    {
        gameObject.GetComponent<ChangeScene>().LoadScene("Perdre");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SceneGagnante()
    {
        gameObject.GetComponent<ChangeScene>().LoadScene("Gagner");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
