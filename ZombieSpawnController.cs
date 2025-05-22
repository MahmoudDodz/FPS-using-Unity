using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class ZombieSpawnController : MonoBehaviour
{
    [Header("Wave Settings")]
    public int intialZombieCountberWaves = 5;
    public int currentZombieberWave;
    public float spawnDelay = 0.5f;
    public float wavecoolDown = 10f;

    [Header("Status")]
    public int currentWave = 0;
    public bool iscoolDown = false;
    public float cooldonwcounter = 0;

    [Header("References")]
    public List<Zombie> currentZombiesAllive = new List<Zombie>();
    public GameObject zombiePrefab;
    public TextMeshProUGUI WaveoverUI;
    public TextMeshProUGUI coolDownUIcounter;
    public TextMeshProUGUI currentwaveUi;

    private void Start()
    {
        currentZombieberWave = intialZombieCountberWaves;
        StartnextWave();
    }

    private void StartnextWave()
    {
        currentZombiesAllive.Clear();
        currentWave++;
        currentwaveUi.text = $"Wave {currentWave}";
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombieberWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            GameObject zombieObj = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Zombie enemyScript = zombieObj.GetComponent<Zombie>();
            currentZombiesAllive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }

        // ⛔ Do not set iscoolDown here anymore — let cooldown start after zombies die
    }

    private void Update()
    {
        // Remove dead zombies
        List<Zombie> zombiesToRemove = new List<Zombie>();
        foreach (Zombie zombie in currentZombiesAllive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        foreach (Zombie zombie in zombiesToRemove)
        {
            currentZombiesAllive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        // Start cooldown if all zombies are dead and not already cooling down
        if (currentZombiesAllive.Count == 0 && !iscoolDown)
        {
            StartCoroutine(WaveCoolDown());
        }

        // Update UI for cooldown timer
        if (iscoolDown)
        {
            cooldonwcounter += Time.deltaTime;
            float remaining = Mathf.Clamp(wavecoolDown - cooldonwcounter, 0, wavecoolDown);
            coolDownUIcounter.text = $"Next Wave: {remaining:F0}s";
        }
        else
        {
            cooldonwcounter = 0f;
            coolDownUIcounter.text = "";
        }
    }

    private IEnumerator WaveCoolDown()
    {
        iscoolDown = true;
        WaveoverUI.gameObject.SetActive(true);

        cooldonwcounter = 0f;
        yield return new WaitForSeconds(wavecoolDown);

        WaveoverUI.gameObject.SetActive(false);
        iscoolDown = false;

        currentZombieberWave *= 2; // Increase difficulty
        StartnextWave();
    }
}
