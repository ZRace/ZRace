using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    public Transform resetPoint;
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponentInParent<vThirdPersonController>())
        {
            other.transform.GetComponentInParent<vThirdPersonController>().transform.position = resetPoint.position;
        }
        else
        {
            Destroy(other.gameObject);
        }

    }
}
