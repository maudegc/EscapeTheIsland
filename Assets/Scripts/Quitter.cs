using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitter : MonoBehaviour
{
    public void Quit()
    {
        Cursor.visible = true;
        Debug.Log("Application quitte");
        Application.Quit();
    }
}
