using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputControl : MonoBehaviour
{
    [SerializeField] private string axis;
    [SerializeField] private UnityEvent keyPressed;
    [SerializeField] private UnityEvent keyReleased;
    [SerializeField] private UnityEvent keyActive;
    [SerializeField] private FloatEvent sendAxisValue;

    private bool _keyPressed;
    private PersonnageCtrl _perso;

    private void Awake()
    {
        _perso = transform.parent.GetComponent<PersonnageCtrl>();
    }

    void Update()
    {
        if (_perso.IsLocalPlayer)
        {
            float axisValue = Input.GetAxis(axis);
            sendAxisValue.Invoke(axisValue);

            if (axisValue != 0)
            {
                if (!_keyPressed)
                {
                    keyPressed.Invoke();
                    _keyPressed = true;
                }
                else
                {
                    keyActive.Invoke();
                }
            }
            else if (_keyPressed)
            {
                keyReleased.Invoke();
                _keyPressed = false;
            }
        }
    }
}