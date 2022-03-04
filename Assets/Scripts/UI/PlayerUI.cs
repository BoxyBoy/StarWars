using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public RectTransform healthBar;
    public RectTransform shieldBar;

    public Player player;

    Transform playerTransform;
    Vector3 playerPosition;
    //Renderer playerRenderer;
    //CanvasGroup mainCanvas;

    float characterControllerHeight = 0f;

    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        //mainCanvas = this.GetComponent<CanvasGroup>();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            Destroy(this.gameObject);
            return;
        }

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

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            playerPosition = playerTransform.position;
            playerPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(playerPosition) + screenOffset;
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        playerTransform = this.player.GetComponent<Transform>();
    }
}
