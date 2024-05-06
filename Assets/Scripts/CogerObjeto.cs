using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerObjeto : MonoBehaviour
{
    public GameObject UbicacionCoger; // Objeto que hereda del brazo del jugador
    public GameObject procesadorPrefab; // Prefab del objeto "procesador"
    public Vector3 armRotationOffset = new Vector3(-77f, 9f, -22f); // Ajuste de la rotación del brazo
    private GameObject pickedObject = null;
    private Transform dogArmRightTransform;
    private Quaternion originalArmLocalRotation; // Guardar la rotación local original del brazo
    private bool nearChest = false; // Variable para verificar si el jugador está cerca de un cofre
    public float radioBusqueda = 5f; // Radio de búsqueda para encontrar bloques cercanos
    public Material colorBloqueCercano; // Material para el bloque más cercano

    // Diccionario para almacenar los materiales originales de los bloques
    private Dictionary<GameObject, Material> materialesOriginales = new Dictionary<GameObject, Material>();

    private void Start()
    {
        // Obtener el transform del objeto llamado "character_dogArmRight"
        dogArmRightTransform = GameObject.Find("character_dogArmRight").transform;

        // Guardar la rotación local original del brazo
        originalArmLocalRotation = dogArmRightTransform.localRotation;
    }

    void Update()
    {
        if (pickedObject != null)
        {
            if (Input.GetKey("r"))
            {
                // Restaurar la rotación local original del brazo
                dogArmRightTransform.localRotation = originalArmLocalRotation;

                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.gameObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
        else
        {
            if (Input.GetKeyDown("e"))
            {

                if (pickedObject == null && UbicacionCoger != null)
                {
                    // Buscar objetos cercanos que se puedan recoger
                    Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 0.7f); // Ajusta el radio según sea necesario

                    foreach (Collider col in nearbyObjects)
                    {
                        if (col.CompareTag("Objeto"))
                        {
                            // Recoger el objeto cercano
                            pickedObject = col.gameObject;
                            pickedObject.transform.SetParent(UbicacionCoger.transform);
                            pickedObject.transform.position = UbicacionCoger.transform.position;

                            // Aplicar rotación fija al brazo
                            dogArmRightTransform.localRotation = Quaternion.Euler(armRotationOffset);

                            // Desactivar la gravedad del objeto recogido
                            Rigidbody pickedRigidbody = pickedObject.GetComponent<Rigidbody>();
                            pickedRigidbody.useGravity = false;
                            pickedRigidbody.isKinematic = true;

                            // Salir del bucle una vez que se haya encontrado y recogido un objeto cercano
                            break;
                        }
                    }

                    // Si no se encontró ningún objeto cercano, instanciar uno nuevo en la UbicacionCoger
                    if (pickedObject == null && nearChest)
                    {
                        // Instanciar el objeto "procesador" en la posición de UbicacionCoger
                        pickedObject = Instantiate(procesadorPrefab, UbicacionCoger.transform.position, Quaternion.identity);
                        pickedObject.transform.SetParent(UbicacionCoger.transform);

                        // Aplicar rotación fija al brazo
                        dogArmRightTransform.localRotation = Quaternion.Euler(armRotationOffset);

                        // Desactivar la gravedad del objeto recogido
                        Rigidbody pickedRigidbody = pickedObject.GetComponent<Rigidbody>();
                        pickedRigidbody.useGravity = false;
                        pickedRigidbody.isKinematic = true;
                    }
                }
            }

        }


        // Encontrar todos los bloques dentro del radio de búsqueda
        Collider[] bloques = Physics.OverlapSphere(transform.position, radioBusqueda);

        // Encontrar el bloque más cercano
        float distanciaMinima = Mathf.Infinity;
        GameObject bloqueCercano = null;

        foreach (Collider bloque in bloques)
        {
            if (bloque.CompareTag("Bloque"))
            {
                float distancia = Vector3.Distance(transform.position, bloque.transform.position);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    bloqueCercano = bloque.gameObject;
                }
            }
        }
        // Cambiar el color del bloque más cercano si la distancia es menor que 1.5
        foreach (Collider bloque in bloques)
        {
            if (bloque.CompareTag("Bloque"))
            {
                GameObject bloqueObjeto = bloque.gameObject;
                Renderer renderer = bloqueObjeto.GetComponent<Renderer>();

                if (bloqueObjeto == bloqueCercano && distanciaMinima < 1.5)
                {
                    // Guardar el material original si no se ha guardado antes
                    if (!materialesOriginales.ContainsKey(bloqueObjeto))
                    {
                        materialesOriginales.Add(bloqueObjeto, renderer.material);
                    }

                    // Cambiar el color del bloque más cercano
                    renderer.material = colorBloqueCercano;
                }
                else
                {
                    // Restaurar el material original si existe
                    if (materialesOriginales.ContainsKey(bloqueObjeto))
                    {
                        renderer.material = materialesOriginales[bloqueObjeto];
                    }
                }
            }
        }
    }

    //Funciones para comprobar si el jugador está o no cerca del cofre
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cofre"))
        {
            nearChest = true; // Establecer la variable a true si el jugador está cerca del cofre
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cofre"))
        {
            nearChest = false; // Establecer la variable a false cuando el jugador sale del área del cofre
        }
    }
}