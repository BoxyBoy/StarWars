using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    float masterVolumePercent = .3f;
    float sfxVolumePercent = 1f;
    float musicVolumePercent = 1f;

    Transform audioListener;
    Transform player;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;

        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();

            newMusicSource.transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (player != null)
        {
            audioListener.position = player.position;
        }
    }

    public void PlayMusic(AudioClip audioClip, float fadeDuration = 1f)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = audioClip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlaySound(AudioClip audioClip, Vector3 position)
    {
        if (audioClip == null) return;
        AudioSource.PlayClipAtPoint(audioClip, position, sfxVolumePercent * masterVolumePercent);
    }

    private IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;

        while (percent < 1f)
        {
            percent += Time.deltaTime * (1f / duration);
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);

            yield return null;
        }
    }
}
