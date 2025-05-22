using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<GameObject> weaponsSlots;
    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponsSlots[0];
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

        foreach (GameObject weaponSlot in weaponsSlots)
        {
            weaponSlot.SetActive(weaponSlot == activeWeaponSlot);
        }
    }

    public void pickupWeapon(GameObject pickedWeapon)
    {
        AddWeaponToActiveSlot(pickedWeapon);
    }

    private void AddWeaponToActiveSlot(GameObject pickedWeapon)
    {
        // Get reference
        Weapon weapon = pickedWeapon.GetComponent<Weapon>();

        // Store the weapon's original ground scale
        weapon.originalScale = pickedWeapon.transform.localScale;

        // Save ground position/rotation
        Vector3 pickupPosition = pickedWeapon.transform.position;
        Quaternion pickupRotation = pickedWeapon.transform.rotation;

        // Drop current weapon to the same position
        DropCurrentWeapon(pickupPosition, pickupRotation);

        // Parent picked weapon to hand/slot
        pickedWeapon.transform.SetParent(activeWeaponSlot.transform, false);
        pickedWeapon.transform.localPosition = weapon.SpawnPointPosition;
        pickedWeapon.transform.localRotation = Quaternion.Euler(weapon.SpawnPointRotation);
        pickedWeapon.transform.localScale = weapon.originalScale; // Preserve ground scale

        weapon.isActiveWeapon = true;

        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(Vector3 dropPosition, Quaternion dropRotation)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            GameObject weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;
            Weapon weaponComp = weaponToDrop.GetComponent<Weapon>();
            weaponComp.isActiveWeapon = false;
            weaponComp.animator.enabled = false;

            // Drop in world space
            weaponToDrop.transform.SetParent(null);
            weaponToDrop.transform.position = dropPosition;
            weaponToDrop.transform.rotation = dropRotation;
            weaponToDrop.transform.localScale = weaponComp.originalScale;
            // Restore its original scale
        }
    }

    public void SwitchWeapon(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.GetComponentInChildren<Weapon>();
            currentWeapon.isActiveWeapon = false;

        }
        activeWeaponSlot = weaponsSlots[slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.GetComponentInChildren<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
}
