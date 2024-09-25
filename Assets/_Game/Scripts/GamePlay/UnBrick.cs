using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBrick : MonoBehaviour
{
    public GameObject brick;
    // xu ly va cham 1 lan
    private bool isCollect = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollect)
        {
            isCollect = true;
            brick.SetActive(true);
            other.GetComponent<Player>().RemoveBrick();
        }
    }
}
