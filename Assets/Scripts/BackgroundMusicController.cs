using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{

    public AudioSource output;
    public Song battle;
    public static Motivation battleMotivation;

    static BackgroundMusicController singleton;

    private void Awake()
    {
        battle.output = output;

        battleMotivation = new Motivation();
        battleMotivation.OnMotivated += battle.PlayBody;
        battleMotivation.OnUnmotivated += battle.PlayFanfare;
        singleton = this;
    }

    public static void Stop()
    {
        if ( singleton != null )
        {
            singleton.output.Stop();
        }
    }

    [System.Serializable]
    public class Song
    {
        [HideInInspector]
        public AudioSource output;
        public AudioClip body;
        public AudioClip endingFanfare;

        public void PlayBody()
        {
            if (output == null) return;
            output.loop = true;
            output.clip = body;
            output.Play();
        }

        public void PlayFanfare()
        {
            if (output == null) return;
            output.loop = false;
            output.clip = endingFanfare;
            output.Play();
        }
    }
}
