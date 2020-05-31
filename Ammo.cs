using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField]
    private float travelSpeed = 5, lifeTime = 3;
    [SerializeField]
    private int damage = 10, startDamage;
    [SerializeField]
    private bool canHitMultiple = false;

    public float GetSpeed()
    {
        return travelSpeed;
    }
    public void SetSpeed(float spd)
    {
        travelSpeed = spd;
    }
    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        startDamage = damage;
        Destroy(this.gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * travelSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("enemy", System.StringComparison.CurrentCultureIgnoreCase))
        {
            ///insert what happens to the enemy when hit HERE
            //example:
            //enemy.GetComponent<HealthManager>().DealDamage(damage);

            if (!canHitMultiple || damage <= 0)
                Destroy(this.gameObject);
            else
            {
                damage -= (int)(startDamage / 5);
            }
        }
    }
}
