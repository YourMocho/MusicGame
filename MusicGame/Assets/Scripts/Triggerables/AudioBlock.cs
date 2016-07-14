using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AudioBlock : Block<AudioClip>, IPointerClickHandler
{
    public Octave Octave { get { return octave; } set { octave = value; } }

    public Note Note { get { return note; } set { note = value; } }

    [SerializeField]
    Octave octave;

    [SerializeField]
    Note note;

    float vibrationSpeed = 30;

    float vibrationSize = 0.0015f;

    float vibrationDuration = 0.3f;

    [SerializeField]
    bool isTriggerableByClick = false;

    float randomOffset;

    protected override void Awake()
    {
        AudioClip clip = SoundLoader.Instance.GetSound(octave, note);

        triggerable = new AudioTriggerable(clip, numberOfTriggerUses, canBeUsedUp);
        triggerable.Setup();
        Triggering += AudioPlayer.Instance.HandleTriggerable;
        Triggering += StartVibrating;

        base.Awake();
    }
    
    void StartVibrating(Triggerable<AudioClip> triggered)
    {
        randomOffset = Random.value;

        StartCoroutine(Vibrate(vibrationDuration));
    }
    
    IEnumerator Vibrate(float duration)
    {
        while (duration > 0)
        {
            float x = Mathf.Sin((Time.time + randomOffset) * vibrationSpeed) * vibrationSize;
            float y = Mathf.Sin(Time.time * vibrationSpeed) * vibrationSize;

            x += transform.position.x;
            y += transform.position.y;

            transform.position = new Vector3(x, y, 0);

            duration -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTriggerableByClick)
        {
            triggerable.Trigger();
        }
    }

    public override void OnUsedUp()
    {
        renderer.enabled = false;
        collider.enabled = false;
    }

    public override void OnReset()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }
}
