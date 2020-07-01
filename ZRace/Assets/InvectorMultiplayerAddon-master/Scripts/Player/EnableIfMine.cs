using Photon.Pun;
using UnityEngine;

public class EnableIfMine : MonoBehaviour
{
    [SerializeField] protected GameObject[] targets = new GameObject[] { };
    protected virtual void Start()
    {
        if (GetComponent<PhotonView>())
        {
            EnableTargets(GetComponent<PhotonView>().IsMine);
        }
        else if (GetComponentInChildren<PhotonView>())
        {
            EnableTargets(GetComponentInChildren<PhotonView>().IsMine);
        }
    }
    protected virtual void EnableTargets(bool isEnabled)
    {
        foreach(GameObject target in targets)
        {
            target.SetActive(isEnabled);
        }
    }
}
