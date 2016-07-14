using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Block<T> : TriggerableHolder<T>
{
    new protected SpriteRenderer renderer;
    new protected BoxCollider2D collider;

    protected override void Awake()
    {
        base.Awake();

        renderer = GetComponent<SpriteRenderer>();
        renderer.color = ColorSettings.Instance.BlockColor;

        collider = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Triggering != null)
        {
            Triggering(triggerable);
        }
    }
}
