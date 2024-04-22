using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
    public GameObject UbicacionCoger;
    private GameObject pickedObject = null;
    private Transform dogArmRightTransform; // Mover la declaración aquí

    private void Start()
    {
        // Obtener el transform del objeto llamado "character_dogArmRight"
        dogArmRightTransform = GameObject.Find("character_dogArmRight").transform; // Asignar el valor aquí
    }

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                // Cambiar la posición brazo
                dogArmRightTransform.rotation = Quaternion.Euler(0f, 0f, 0f);

                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Objeto"))
        {
            Debug.Log("¡Hola, mundo!");
            if (Input.GetKey("e") && pickedObject == null)
            {
                // Cambiar la posición brazo
                dogArmRightTransform.rotation = Quaternion.Euler(-77f, 9f, -22f);

                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = UbicacionCoger.transform.position;
                other.gameObject.transform.SetParent(UbicacionCoger.gameObject.transform);
                pickedObject = other.gameObject;
            }
        }
    }
}
