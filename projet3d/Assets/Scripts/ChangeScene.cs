using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    private void SubmitSwitchSceneServerRpc(string sceneName, ServerRpcParams rpcParams = default)
    {
        NetworkSceneManager.SwitchScene(sceneName);
        //SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            //SceneManager.LoadScene(sceneName);          
            NetworkSceneManager.SwitchScene(sceneName);
        }
        else 
        {
            SubmitSwitchSceneServerRpc(sceneName);
        } 
    }

    public void LoadSceneIndependante(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void ReloadScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene(scene.name);

        //JeuCtrl game = FindObjectOfType<JeuCtrl>();
        SceneManager.UnloadScene("ScenePrincipale");
     
    }
}
