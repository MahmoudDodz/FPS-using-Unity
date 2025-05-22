using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource ShootingSoundWeapon1;
    public AudioSource RealoadingSoundWeapon1;

    public AudioSource EmptySoundWeapon1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
