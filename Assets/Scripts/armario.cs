using UnityEngine;

public class Armario : MonoBehaviour
{
    public GameObject objetoAEntregar; // Objeto que se entregará al jugador

    void Start()
    {
        // Suscribirse al evento de entrega del objeto
        ControlObjeto.OnEntregarObjeto += EntregarObjeto;
    }

    void OnDestroy()
    {
        // Darse de baja del evento de entrega del objeto al destruir el objeto
        ControlObjeto.OnEntregarObjeto -= EntregarObjeto;
    }

    void EntregarObjeto()
    {
        // Verificar si el objeto a entregar existe
        if (objetoAEntregar != null)
        {
            // Instanciar el objeto a entregar en la posición del armario
            GameObject objetoEntregado = Instantiate(objetoAEntregar, transform.position, Quaternion.identity);

            // Encontrar la transformada de character_dog en el mismo nivel que el Armario
            Transform parentTransform = transform.parent;
            Transform characterDog = parentTransform.Find("character_dog");

            if (characterDog != null)
            {
                // Hacer que el objeto entregado sea hijo de character_dog
                objetoEntregado.transform.parent = characterDog;
            }
            else
            {
                Debug.LogWarning("No se encontró el objeto character_dog.");
            }

            // Debugging: Imprimir la cantidad de hijos del GameObject padre
            Debug.Log("Cantidad de hijos del GameObject padre: " + parentTransform.childCount);
        }
        else
        {
            Debug.LogWarning("El objeto a entregar no está asignado en el Armario.");
        }
    }





    // Método para encontrar un objeto hijo de manera recursiva
    Transform FindDeepChild(Transform parent, string name)
    {
        Transform result = null;
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                result = child;
                break;
            }
            result = FindDeepChild(child, name);
            if (result != null)
                break;
        }
        return result;
    }



}
