using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    int shieldAmmount = 20;

    protected override void ApplyPowerUp()
    {
        Debug.Log($"Shield Collision, added: {shieldAmmount}");
        player.shield += shieldAmmount;
        if (player.shield > 100)
        {
            player.shield = 100;
        }
        Debug.Log($"Player shield: {player.shield}");
    }
}
