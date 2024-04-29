using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : NetworkBehaviour
{
    public void PausePartie()
    {
        Time.timeScale = 0;
    }

    public void ContinuePartie()
    {
        Time.timeScale = 1;
    }
}
