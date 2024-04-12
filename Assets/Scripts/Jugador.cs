using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float radioBusqueda = 5f; // Radio de búsqueda para encontrar bloques cercanos
    public Material colorBloqueCercano; // Material para el bloque más cercano

    // Diccionario para almacenar los materiales originales de los bloques
    private Dictionary<GameObject, Material> materialesOriginales = new Dictionary<GameObject, Material>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal ,0, moveVertical);
        movement.Normalize();

        transform.position = transform.position + movement * speed * Time.deltaTime;
        if(movement!=Vector3.zero)transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(movement),rotationSpeed * Time.deltaTime);



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

}
