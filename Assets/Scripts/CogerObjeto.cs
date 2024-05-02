using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
    public GameObject UbicacionCoger;
    public GameObject procesadorPrefab; // Prefab del objeto "procesador"
    public Vector3 armRotationOffset = new Vector3(-77f, 9f, -22f); // Ajuste de la rotaci�n del brazo
    private GameObject pickedObject = null;
    private Transform dogArmRightTransform;
    private Quaternion originalArmLocalRotation; // Guardar la rotaci�n local original del brazo
    private Transform characterTransform; // Transform del personaje

    private void Start()
    {
        // Obtener el transform del objeto llamado "character_dogArmRight"
        dogArmRightTransform = GameObject.Find("character_dogArmRight").transform;
        // Guardar la rotaci�n local original del brazo
        originalArmLocalRotation = dogArmRightTransform.localRotation;
        // Obtener el transform del personaje
        characterTransform = transform;
    }

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                // Restaurar la rotaci�n local original del brazo
                dogArmRightTransform.localRotation = originalArmLocalRotation;

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
            if (Input.GetKeyDown("e") && pickedObject == null)
            {
                // Aplicar rotaci�n fija al brazo
                dogArmRightTransform.localRotation = Quaternion.Euler(armRotationOffset);

                // Instanciar el objeto "procesador" en la mano del jugador
                pickedObject = Instantiate(procesadorPrefab, dogArmRightTransform.position, Quaternion.identity);
                pickedObject.transform.SetParent(dogArmRightTransform);
            }
        }

        if (other.CompareTag("Cofre") && Input.GetKeyDown(KeyCode.E))
        {
            // Si no hay un objeto en la mano, instanciar un procesador en la mano del jugador
            if (pickedObject == null)
            {
                pickedObject = Instantiate(procesadorPrefab, dogArmRightTransform.position, Quaternion.identity);
                pickedObject.transform.SetParent(dogArmRightTransform);
            }
            // Si hay un objeto en la mano, soltarlo en el cofre
            else
            {
                pickedObject.transform.SetParent(null); // Liberar el objeto de la mano del jugador
                pickedObject.transform.position = UbicacionCoger.transform.position; // Colocar el objeto en la posici�n del cofre
                pickedObject.GetComponent<Rigidbody>().useGravity = false; // Desactivar la gravedad para que el objeto no caiga
                pickedObject.GetComponent<Rigidbody>().isKinematic = true; // Hacer el objeto cinem�tico para que no responda a la f�sica
                pickedObject = null; // Limpiar la referencia al objeto
            }
        }
    }
}
