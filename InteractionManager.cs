using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    public Weapon hoveredWeapon = null;

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

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Weapon newHoveredWeapon = null;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            newHoveredWeapon = objectHit.GetComponent<Weapon>();

            if (newHoveredWeapon != null && !newHoveredWeapon.isActiveWeapon)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.pickupWeapon(objectHit);
                }
            }
            else
            {
                newHoveredWeapon = null; // ignore active weapons
            }
        }

        if (hoveredWeapon != newHoveredWeapon)
        {
            if (hoveredWeapon != null)
            {
                Outline outline = hoveredWeapon.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }

            if (newHoveredWeapon != null)
            {
                Outline outline = newHoveredWeapon.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }

            hoveredWeapon = newHoveredWeapon;
        }
    }
}
