using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteNotification : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        int countdown = 5;
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1);
            countdown--;
        }

        Destroy(gameObject);
    }
}
