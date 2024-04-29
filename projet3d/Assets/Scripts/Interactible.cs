using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Outline))]
public class Interactible : MonoBehaviour
{
    [SerializeField] private UnityEvent activer;
  

    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void Activer()
    {
        
        activer.Invoke();
    }


    public void Selectionner()
    {
        _outline.enabled = true;
    }

    public void Deselectionner()
    {
        _outline.enabled = false;
    }

}
