using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int WeaponDamage;
    public bool isActiveWeapon;
    public Vector3 SpawnPointPosition;
    public Vector3 SpawnPointRotation;
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    public int bulletperburst = 3;
    public int BulletLeft;
    public float spreadintensity;
    public GameObject MuzzleEffect;
    internal Animator animator;
    public float ReloadTime;
    public int magazineSize;
    public bool isReloading;
    public Vector3 originalScale;

    public enum WeaponType
    {
        M16,
        AK74,
    }

    public WeaponType thisWeaponType;
    public enum shootingMode
    {
        Single,
        Burst,
        Auto
    }

    public shootingMode currentShootingMode;
    private void Awake()
    {
        readyToShoot = true;
        BulletLeft = bulletperburst;
        animator = GetComponent<Animator>();
        BulletLeft = magazineSize;

    }



    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 100f;
    public float bulletprefablifetime = 3f;
    void Update()
    {
        if (isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");

            }
            GetComponent<Outline>().enabled = false;

            if (BulletLeft == 0 && isShooting)
            {
                SoundManager.Instance.EmptySoundWeapon1.Play();
            }
            if (currentShootingMode == shootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == shootingMode.Single || currentShootingMode == shootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && BulletLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && BulletLeft <= 0 && !isReloading)
            {
                Reload();
            }

            if (isShooting && readyToShoot && BulletLeft > 0)
            {
                FireWeapon();
            }
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");

                }
            }


        }
    }
    private void Reload()
    {
        SoundManager.Instance.RealoadingSoundWeapon1.Play();
        animator.SetTrigger("RELOAD");


        isReloading = true;
        Invoke("ReloadCompleted", ReloadTime);

    }
    private void ReloadCompleted()
    {

        BulletLeft = magazineSize;
        isReloading = false;
    }
    private void FireWeapon()
    {
        BulletLeft--;
        MuzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.ShootingSoundWeapon1.Play();
        readyToShoot = false;
        Vector3 shootingDirection = calculateShootingDirectionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.BulletDamage = WeaponDamage;

        bullet.transform.forward = shootingDirection;


        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletprefablifetime));

        if (allowReset)
        {
            Invoke("ResetShooting", shootingDelay);
            allowReset = false;
        }
        if (currentShootingMode == shootingMode.Burst && BulletLeft > 1)
        {
            BulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }



    }


    private void ResetShooting()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 calculateShootingDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawnPoint.position;
        float x = UnityEngine.Random.Range(-spreadintensity, spreadintensity);
        float y = UnityEngine.Random.Range(-spreadintensity, spreadintensity);
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

}
