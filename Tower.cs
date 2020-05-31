using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public List<EnemyInfo> enemiesInRange = new List<EnemyInfo>();
    [SerializeField]
    private GameObject turretTopPart;
    private Quaternion topStartRot;

    [SerializeField]
    private SphereCollider viewDistanceSphere;

    [SerializeField]
    private float turnSpeed = 3, aimForwardOffset = 2, viewDistance = 10, fireRate = 1, environmentalDestructionRadius = 2, extraAmmoSpeed = 0;
    [SerializeField]
    private EnemyInfo target;

    [SerializeField]
    private GameObject myAmmo;
    [SerializeField]
    private Transform ammoOriginPoint;
    [SerializeField]
    private bool canFire = true;

    public void SetAmmo(GameObject ammo)
    {
        if (ammo.GetComponent<Ammo>())
            myAmmo = ammo;
    }
    public GameObject GetAmmo()
    {
        return myAmmo;
    }
    public void SetFireRate(float rate)
    {
        if (rate > 0)
            fireRate = rate;
    }
    public float GetEnvironmentalDestructionRadius()
    {
        return environmentalDestructionRadius;
    }
    void Clear()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] == null)
                enemiesInRange.RemoveAt(i);
        }
    }

    void Start()
    {
        if (!viewDistanceSphere)
            viewDistanceSphere = GetComponent<SphereCollider>();

        viewDistanceSphere.radius = viewDistance;

        StartCoroutine(RazeSurroundings());

        topStartRot = turretTopPart.transform.rotation;

    }

    void Update()
    {
        AimAtTarget();
    }
    void AssignTarget()
    {
        Clear();
        if (!target)
        {
            if (enemiesInRange.Count > 0)
            {
                target = enemiesInRange[0];
            }
        }
    }
    void AimAtTarget()
    {
        AssignTarget();
        if(target)
        {
            aimForwardOffset = target.Movement().GetAgent().velocity.magnitude * (Vector3.Distance(transform.position, target.transform.position) / 10);
            Vector3 dir = transform.transform.position - (target.transform.position + target.transform.forward * -aimForwardOffset);
            dir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(-dir, Vector3.up);
            RotateInDirection(lookRot);

            if (canFire)
                StartCoroutine(Fire());

            if (Vector3.Distance(transform.position, target.transform.position) > viewDistance + 2.5f || !target.Health().IsAlive())
                RemoveTarget();
        }
        else
        {
            RotateInDirection(topStartRot);
        }
    }
    void RemoveTarget()
    {
        if(target)
        {
            if (enemiesInRange.Contains(target))
                enemiesInRange.Remove(target);
        }
        if (enemiesInRange.Count > 0)
            target = enemiesInRange[enemiesInRange.Count - 1]; // target the last enemy that entered range
        else
            target = null;
    }
    IEnumerator Fire()
    {
        canFire = false;

        yield return new WaitForSeconds(fireRate);

        if (!target)
        {
            canFire = true;
            yield break;
        }

        this.GetComponent<AudioSource>().Play();
        GameObject newAmmo = GameObject.Instantiate(myAmmo);
        Ammo nA = newAmmo.GetComponent<Ammo>();
        newAmmo.transform.position = ammoOriginPoint.transform.position;
        newAmmo.transform.rotation = turretTopPart.transform.rotation;
        nA.SetSpeed(nA.GetSpeed() + extraAmmoSpeed);
        nA.SetDamage(nA.GetDamage() + (int)extraAmmoSpeed);

        canFire = true;
    }
    IEnumerator RazeSurroundings()
    {
        viewDistanceSphere.enabled = false;
        yield return new WaitForEndOfFrame();
        viewDistanceSphere.enabled = true;
    }
    void RotateInDirection(Quaternion lookRotation)
    {
        turretTopPart.transform.rotation = Quaternion.Slerp(turretTopPart.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("enemy", System.StringComparison.CurrentCultureIgnoreCase))
        {
            if (other.GetComponent<EnemyInfo>())
                enemiesInRange.Add(other.GetComponent<EnemyInfo>());
        }
        if(other.tag.Equals("removable", System.StringComparison.CurrentCultureIgnoreCase))
        {
            if(Vector3.Distance(transform.position, other.transform.position) <= environmentalDestructionRadius)
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
                enemiesInRange.Remove(other.GetComponent<EnemyInfo>());
        }
    }


}
