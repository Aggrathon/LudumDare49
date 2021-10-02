using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class SirtetBlock : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 targetPos;
    float targetRot;
    bool move;
    bool rotate;
    bool down;
    Vector2 velPos;
    float velRot;

    public float speed = 3.0f;
    public float fastMult = 2.0f;
    public float moveSpeed = 4.0f;
    public float rotSpeed = 180.0f;
    public ColorPalette palette;

    private void OnEnable()
    {
        move = false;
        rotate = false;
        down = false;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
        Color c = palette.Sample();
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = c;
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            down = false;
        }
        else if (context.started)
        {
            down = true;
        }
    }

    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var target = rb.position;
            target.x = Mathf.Round(target.x) + 1.0f;
            targetPos = target;
            move = true;
        }
    }

    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var target = rb.position;
            target.x = Mathf.Round(target.x) - 1.0f;
            targetPos = target;
            move = true;
        }
    }

    public void RotateLeft(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            targetRot = Mathf.Round(rb.rotation / 90f) * 90f - 90f;
            rotate = true;
        }
    }

    public void RotateRight(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            targetRot = Mathf.Round(rb.rotation / 90f) * 90f + 90f;
            rotate = true;
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Disable();
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            var pos = Vector2.SmoothDamp(rb.position, targetPos, ref velPos, 1 / moveSpeed, moveSpeed * 2, Time.fixedDeltaTime);
            if (Vector2.SqrMagnitude(pos - targetPos) < 0.0001)
            {
                rb.MovePosition(targetPos);
                move = false;
            }
            else
            {
                rb.MovePosition(pos);
            }
        }
        if (rotate)
        {
            var rot = Mathf.SmoothDampAngle(rb.rotation, targetRot, ref velRot, 1 / rotSpeed, rotSpeed * 2, Time.deltaTime);
            if (Mathf.Abs(rot - targetRot) < 0.001)
            {
                rb.MoveRotation(targetRot);
                rotate = false;
            }
            else
            {
                rb.MoveRotation(rot);
            }

        }
        if (!move && !rotate)
        {
            if (down)
                rb.velocity = new Vector2(0, -speed * fastMult);
            else
                rb.velocity = new Vector2(0, -speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Disable();
    }

    void Disable()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.WakeUp();
        this.enabled = false;
    }
}
