using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform shieldBar;

    public GameEntity player;

    void FixedUpdate()
    {
        //Update Health
        float healthPercent = 0f;
        healthPercent = player.health / player.initialHealth;
        healthBar.localScale = new Vector3(healthPercent, 1f, 1f);

        float shieldPercent = 0f;
        shieldPercent = player.shield / player.maxShield;
        shieldBar.localScale = new Vector3(shieldPercent, 1f, 1f);

        if(player = null)
        {
            Destroy(this);
        }
    }

    public void SetPlayer(GameEntity player)
    {
        this.player = player;
    }
}
