using UnityEngine;
using UnityEngine.AI;

/*	
	This component is for all objects that the player can
	interact with such as enemies, items etc. It is meant
	to be used as a base class.
*/

//[RequireComponent(typeof(ColorOnHover))]
public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;

    // This method is meant to be overwritten
    public virtual void Interact()
    {
        Debug.Log("interacting with: " + transform.name);

    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }
        Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }





}