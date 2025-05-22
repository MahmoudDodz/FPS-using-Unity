using UnityEngine;

public class GlobalRefrance : MonoBehaviour
{
    public static GlobalRefrance Instance { get; private set; }
    public GameObject bulletImpactbulletPrefab;

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
