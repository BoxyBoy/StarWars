using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPowerUp : PowerUp
{
    int ammoAmmount = 20;

    protected override void ApplyPowerUp()
    {
        Debug.Log($"Ammo Collision, added: {ammoAmmount}");
        player.gunController.equippedGun.ammoCount += ammoAmmount;
        Debug.Log($"Gun ammo: {player.gunController.equippedGun.ammoCount}");
    }
}
