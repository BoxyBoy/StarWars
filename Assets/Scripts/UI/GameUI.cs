using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;

	private void Start () {
        Player player = FindObjectOfType<Player>();

        player.OnDeath += OnGameOver;
	}
	
    private void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1f, 2f));
        gameOverUI.SetActive(true);
    }

    private IEnumerator Fade(Color from, Color to, float time, float delay)
    {
        yield return new WaitForSeconds(delay);

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
}
