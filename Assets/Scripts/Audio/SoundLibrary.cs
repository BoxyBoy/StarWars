using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {

    public SoundGroup[] soundGroups;

    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        foreach (SoundGroup soundGroup in soundGroups)
        {
            groupDictionary.Add(soundGroup.groupID, soundGroup.groupClips);
        }
    }

    public AudioClip GetClipFromName(string clipName)
    {
        if (groupDictionary.ContainsKey(clipName))
        {
            AudioClip[] groupSounds = groupDictionary[clipName];
            return groupSounds[Random.Range(0, groupSounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string groupID;
        public AudioClip[] groupClips;
    }
}
