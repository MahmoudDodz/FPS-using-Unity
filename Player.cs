using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bloodScreen;
    public int HP = 100;
    public bool isDead = false;

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // Prevent taking damage after death

        HP -= damageAmount;
        if (HP <= 0)
        {
            isDead = true;
            Debug.Log("Player is dead");
            PlayerDead();
        }
        else
        {
            Debug.Log("Player took damage");
            StartCoroutine(BloodScreen());
        }
    }

    private void PlayerDead()
    {
        // Disable movement and look
        GetComponent<MouseMoving>().enabled = false;
        GetComponent<Playermovement>().enabled = false;

        // Optionally play death animation if set up
        GetComponentInChildren<Animator>().enabled = true;
    }

    private IEnumerator BloodScreen()
    {
        if (!bloodScreen.activeInHierarchy)
        {
            bloodScreen.SetActive(true);
        }

        var image = bloodScreen.GetComponentInChildren<Image>();

        // Set initial alpha to 1 (fully visible)
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bloodScreen.activeInHierarchy)
        {
            bloodScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called with object: {other.name}, tag: {other.tag}");

        if (other.CompareTag("ZombieHand"))
        {
            Debug.Log("ZombieHand hit detected — calling TakeDamage");
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
