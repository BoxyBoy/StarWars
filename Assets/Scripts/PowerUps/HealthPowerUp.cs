using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
    int healAmmount = 20;

    protected override void ApplyPowerUp()
    {
        Debug.Log($"Heal Collision, healed: {healAmmount}");
        player.health += healAmmount;
        Debug.Log($"Player health: {player.health}");
    }

}
