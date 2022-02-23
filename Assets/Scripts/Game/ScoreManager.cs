using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public static int score { get; private set; }

    Player player;
    float lastEnemyKilledTime;
    float streakExpiryTime = 1f;
    int currentStreakCount;

    private void Start()
    {
        score = 0;
        Enemy.OnDeathStatic += OnEnemyKilled;

        player = FindObjectOfType<Player>();
        player.OnDeath += OnPlayerDeath;
    }

    public void OnEnemyKilled()
    {
        if (Time.time < lastEnemyKilledTime + streakExpiryTime)
        {
            currentStreakCount++;
        }
        else
        {
            currentStreakCount = 0;
        }

        lastEnemyKilledTime = Time.time;
        score += 5 + (int)Mathf.Pow(2, currentStreakCount);
    }

    private void OnPlayerDeath()
    {
        Enemy.OnDeathStatic -= OnEnemyKilled;
    }
}
