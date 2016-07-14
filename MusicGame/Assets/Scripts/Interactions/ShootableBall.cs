using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ShootableBall : SnappingDragAwayItem
{
    [SerializeField]
    float shootSpeed = 1;

    new Rigidbody2D rigidbody;
    new SpriteRenderer renderer;
    new CircleCollider2D collider;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();

        Deselected += ShootBall;

        Show(false);
    }

    void ShootBall(Vector2 endPosition)
    {
        Vector2 moveVector = (startPosition - endPosition);

        if (moveVector.magnitude > PlayerSettings.Instance.MinDragDistance)
        {
            if (moveVector.magnitude > PlayerSettings.Instance.MaxDragDistance)
            {
                moveVector = moveVector.normalized * PlayerSettings.Instance.MaxDragDistance;
            }

            rigidbody.AddForce(moveVector * shootSpeed / Screen.width);
        }
        else
        {
            Reset();
        }
    }

    public override void Reset()
    {
        base.Reset();

        rigidbody.velocity = Vector3.zero;
    }

    public override void Show(bool isVisible)
    {
        collider.enabled = isVisible;
        renderer.enabled = isVisible;
    }
}
