using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe permettant de faire tomber un itemm
/// </summary>
public class BlesserItem : MonoBehaviour
{
    [SerializeField] int _barreDeVie = 5;
    [SerializeField] int _tempsDisparition = 5;
  
    [SerializeField] private UnityEvent _mourir;
    [SerializeField] private UnityEvent _blesser;

    private Rigidbody _rb;
    private GameObject _joueur;
    
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _joueur = GameObject.FindWithTag("Player");
    }
   
    void Update()
    {
        if (_barreDeVie <= 0)
        {
            _mourir.Invoke();
        }
    }

    /// <summary>
    /// Permet de faire tomber dans la direction que le joueur le pousse
    /// Detruit ensuite l'item
    /// </summary>
    public void Mourir()
    {
        _rb.isKinematic = false;
        if (_joueur == null)
        {
            _joueur = GameObject.FindWithTag("Player");
        }
        _rb.AddForce(_joueur.transform.forward, ForceMode.Impulse);
        Destroy(gameObject, _tempsDisparition);
    }

    /// <summary>
    /// Appele une activité si elle n'est pas nul et enlève un point de vie à l'item
    /// </summary>
    public void Blesser()
    {
        if (_blesser != null)
        {
            _blesser.Invoke();
        }
        _barreDeVie--;
    }
}
