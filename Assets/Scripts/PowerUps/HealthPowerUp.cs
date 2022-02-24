using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
    int healAmmount = 5;

    protected override void ApplyPowerUp()
    {
        player.health += healAmmount;
    }

}
