using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Ammo")]
    public TextMeshProUGUI MagazineammoUI;   // Shows current ammo
    public TextMeshProUGUI TotalammoUI;      // Shows magazine size or reserve
    public Image ammotypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unactiveWeaponUI;

    [Header("Throwable")]
    public Image LethaUI;
    public TextMeshProUGUI LethacountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalcountUI;

    public Sprite emptySprite;

    // Cached icons from prefab SpriteRenderers
    private Sprite ak74Icon;
    private Sprite m16Icon;
    private Sprite ak74Ammo;
    private Sprite m16Ammo;

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

        // ✅ Load sprites once from prefab-based SpriteRenderers
        ak74Icon = LoadSpriteFromPrefab("AK74");
        m16Icon = LoadSpriteFromPrefab("M16");
        ak74Ammo = LoadSpriteFromPrefab("AK74A");
        m16Ammo = LoadSpriteFromPrefab("M16A");
    }

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        GameObject unactiveSlot = GetUnactiveWeaponSlot();
        Weapon unactiveWeapon = unactiveSlot ? unactiveSlot.GetComponentInChildren<Weapon>() : null;

        if (activeWeapon)
        {
            // ✅ Show current and max ammo cleanly
            MagazineammoUI.text = $"{activeWeapon.BulletLeft} / {activeWeapon.magazineSize}";
            TotalammoUI.text = ""; // Optional: use this for reserve ammo later

            // ✅ Assign icons
            ammotypeUI.sprite = GetAmmoSprite(activeWeapon.thisWeaponType);
            activeWeaponUI.sprite = GetWeaponSprite(activeWeapon.thisWeaponType);

            if (unactiveWeapon)
                unactiveWeaponUI.sprite = GetWeaponSprite(unactiveWeapon.thisWeaponType);
            else
                unactiveWeaponUI.sprite = emptySprite;
        }
        else
        {
            // No weapon equipped – clear UI
            MagazineammoUI.text = "";
            TotalammoUI.text = "";
            ammotypeUI.sprite = emptySprite;
            activeWeaponUI.sprite = emptySprite;
            unactiveWeaponUI.sprite = emptySprite;
        }
    }

    private GameObject GetUnactiveWeaponSlot()
    {
        foreach (GameObject slot in WeaponManager.Instance.weaponsSlots)
        {
            if (slot != WeaponManager.Instance.activeWeaponSlot)
                return slot;
        }
        return null;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponType type)
    {
        return type switch
        {
            Weapon.WeaponType.AK74 => ak74Icon,
            Weapon.WeaponType.M16 => m16Icon,
            _ => emptySprite
        };
    }

    private Sprite GetAmmoSprite(Weapon.WeaponType type)
    {
        return type switch
        {
            Weapon.WeaponType.AK74 => ak74Ammo,
            Weapon.WeaponType.M16 => m16Ammo,
            _ => emptySprite
        };
    }

    private Sprite LoadSpriteFromPrefab(string prefabName)
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>("Sprites/" + prefabName));
        SpriteRenderer sr = temp.GetComponent<SpriteRenderer>();
        Sprite result = sr ? sr.sprite : emptySprite;
        Destroy(temp); // Prevent scene clutter
        return result;
    }
}
