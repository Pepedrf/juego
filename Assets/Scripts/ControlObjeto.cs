using System;
using UnityEngine;

public class ControlObjeto : MonoBehaviour
{
    public static event Action OnEntregarObjeto; // Evento para entregar el objeto

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (OnEntregarObjeto != null)
            {
                OnEntregarObjeto(); // Activar el evento
            }
        }
    }
}
