using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class Bullet : MonoBehaviour
{
    public int BulletDamage = 10;
    private void OnCollisionEnter(Collision collision)

    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name);
            creatBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit wall");
            creatBulletImpactEffect(collision);

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {
            print("hit Zombie");
            collision.gameObject.GetComponent<Zombie>().TakeDamage(BulletDamage);
            Destroy(gameObject);
        }

    }
    void creatBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GlobalRefrance.Instance.bulletImpactbulletPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectWeHit.transform);
    }
}
