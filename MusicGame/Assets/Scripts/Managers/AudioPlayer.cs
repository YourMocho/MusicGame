using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : TriggerHandler<AudioPlayer, AudioClip>
{
    List<AudioSource> audioPlayers = new List<AudioSource>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

	public void PlayAudio(AudioClip audio)
    {
        AudioSource player = gameObject.AddComponent<AudioSource>();
        player.clip = audio;
        player.Play();
        audioPlayers.Add(player);
	}

    void Update()
    {
        for (int i = audioPlayers.Count - 1; i >= 0; i--)
        {
            AudioSource player = audioPlayers[i];
            player.volume = AudioSettings.Instance.Volume;
            if (player.time >= player.clip.length)
            {
                Destroy(player);
                audioPlayers.Remove(player);
            }
        }
    }
}
