using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerViewSphere : MonoBehaviour
{
    public Tower myTower;
    // Start is called before the first frame update
    void Start()
    {
        if (!myTower)
            myTower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("enemy", System.StringComparison.CurrentCultureIgnoreCase))
        {
            if (other.GetComponent<EnemyInfo>())
                myTower.enemiesInRange.Add(other.GetComponent<EnemyInfo>());
        }
        if (other.tag.Equals("removable", System.StringComparison.CurrentCultureIgnoreCase))
        {
            if (Vector3.Distance(transform.position, other.transform.position) <= myTower.GetEnvironmentalDestructionRadius())
            {
                Destroy(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("enemy", System.StringComparison.CurrentCultureIgnoreCase))
        {
            if (other.GetComponent<EnemyInfo>())
                myTower.enemiesInRange.Remove(other.GetComponent<EnemyInfo>());
        }
    }
}
