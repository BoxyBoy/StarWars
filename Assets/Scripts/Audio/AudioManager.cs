using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public enum AudioChannel { MASTER, SFX, MUSIC }

    SoundLibrary soundLibrary;

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }

    Transform audioListener;
    Transform player;

    AudioSource[] musicSources;
    AudioSource sfx2DSource;
    int activeMusicSourceIndex;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (player == null)
        {
            if (FindObjectOfType<Player>() != null)
            {
                player = FindObjectOfType<Player>().transform;
            }
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        soundLibrary = GetComponent<SoundLibrary>();

        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        // SFX AudioSource
        GameObject newSfx2DSource = new GameObject("2D SFX Source");
        sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
        newSfx2DSource.transform.parent = transform;

        audioListener = FindObjectOfType<AudioListener>().transform;
        if (FindObjectOfType<Player>())
        {
            player = FindObjectOfType<Player>().transform;
        } 

        // Load Player Preferences
        masterVolumePercent = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolumePercent = PlayerPrefs.GetFloat("SfxVolume", 1f);
        musicVolumePercent = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    private void Update()
    {
        if (player != null)
        {
            audioListener.position = player.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.MASTER:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.SFX:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.MUSIC:
                musicVolumePercent = volumePercent;
                break;
        }

        musicSources[activeMusicSourceIndex].volume = musicVolumePercent * masterVolumePercent;

        // Save Player Preferences
        PlayerPrefs.SetFloat("MasterVolume", masterVolumePercent);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolumePercent);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip audioClip, float fadeDuration = 1f)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = audioClip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossFade(fadeDuration));
    }

    public void PlaySound(string soundName, Vector3 position)
    {
        PlaySound(soundLibrary.GetClipFromName(soundName), position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position)
    {
        if (audioClip == null) return;
        AudioSource.PlayClipAtPoint(audioClip, position, sfxVolumePercent * masterVolumePercent);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(soundLibrary.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
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
