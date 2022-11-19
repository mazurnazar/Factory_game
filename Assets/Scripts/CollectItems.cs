using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    [SerializeField] private Player bag;
    public bool collecting = false, giving = false;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Collect")
        {
            if (!collecting)
            {
                collecting = true;
                StartCoroutine(bag.TakeItems(other.gameObject.GetComponent<WareHouse>()));
            }

        }
        // if collides with trigge of deliver -> give items to collided object
        if (other.gameObject.tag == "Deliver")
        {
            if (!giving)
            {
                giving = true;
                StartCoroutine(bag.GiveItems(other.GetComponent<WareHouse>()));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        collecting = false;giving = false;
        StopAllCoroutines();
    }
}
