using UnityEngine;

public class Zombie2 : MonoBehaviour
{
    public ZombieHand zombieHand;
    public int ZombieDamage = 25;
    public void Start()
    {
        zombieHand.damage = ZombieDamage;

    }

}
