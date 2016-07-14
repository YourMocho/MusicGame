using UnityEngine;
using System.Collections;

public class AudioTriggerable : Triggerable<AudioClip>
{
    public AudioTriggerable(AudioClip triggerItem, int uses, bool canBeUsedUp) : base(triggerItem, uses, canBeUsedUp)
    {
    }

    public override void Setup()
    {
        Use += AudioPlayer.Instance.PlayAudio;
    }

    public override void Trigger()
    {
        base.Trigger();
    }
}
