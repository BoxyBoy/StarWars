using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SingleplayerGameUI : MonoBehaviour
{
    public Image fadePlane;
    public GameObject gameOverUI;

    public GameObject healthBars;
    public Text ammoText;

    public SquadController squad;
    public Player player;

    private void Start()
    {
        player = squad.focusPlayer;
    }

    private void Update()
    {
        if (player == null)
        {
            player = squad.focusPlayer;
        }

        if (squad.numSquadies == 1)
        {
            player.OnDeath += OnGameOver;
        }

        ammoText.text = "Ammo: " + player.gunController.equippedGun.projectilesRemainingInMagazine + "/" + player.gunController.equippedGun.ammoCount.ToString();

        //health is handeled in player.cs
    }


    private void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, .7f), 2f));
        healthBars.transform.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI Input
    public void StartNewGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
