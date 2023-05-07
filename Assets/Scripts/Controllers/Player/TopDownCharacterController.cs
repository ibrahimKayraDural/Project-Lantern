using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownCharacterController : MonoBehaviour
{
    public event EventHandler<bool> Event_OnDeath;

    [Header("Reference")]
    [SerializeField] GameObject[] DisableOnDeath;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform transformToFlip;

    [Header("Variables")]
    [SerializeField] float speed = 20f;

    bool isLookingLeft;
    int health = 2;

    float PressTime_left, PressTime_right, PressTime_up, PressTime_down;

    void Update()
    {
        Vector2 movementVector = GetInputVector();

        bool anim_isMoving = Mathf.Abs(movementVector.x) > .1f || Mathf.Abs(movementVector.y) > .1f;

        movementVector *= speed * Time.deltaTime;

        if (isLookingLeft && movementVector.x > 0) Flip();
        else if (isLookingLeft == false && movementVector.x < 0) Flip();

        Vector2 targetPosition = (Vector2)transform.position + movementVector;

        //Apply animation
        animator.SetBool("isMoving", anim_isMoving);

        rb.MovePosition(targetPosition);
    }

    public void GetDamaged()
    {
        health--;

        if (health <= 0) Die();
    }
    void Die()
    {
        Event_OnDeath?.Invoke(this, true);
        foreach(GameObject go in DisableOnDeath)
        {
            go.SetActive(false);
        }

        this.enabled = false;
    }

    Vector2 GetInputVector()
    {
        Vector2 inputVector = Vector2.zero;

        bool IsHolding_left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool IsHolding_right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool IsHolding_up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool IsHolding_down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { PressTime_left = Time.time; }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { PressTime_right = Time.time; }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { PressTime_up = Time.time; }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { PressTime_down = Time.time; }

        if (IsHolding_left) inputVector.x = -1;
        if (IsHolding_right) inputVector.x = 1;
        if (IsHolding_up) inputVector.y = 1;
        if (IsHolding_down) inputVector.y = -1;

        if (IsHolding_left && IsHolding_right) inputVector.x = PressTime_left >= PressTime_right ? -1 : 1;
        if (IsHolding_up && IsHolding_down) inputVector.y = PressTime_up >= PressTime_down ? 1 : -1;

        inputVector.Normalize();

        return inputVector;
    }

    void Flip()
    {
        transformToFlip.localScale =
            new Vector3(transformToFlip.localScale.x * -1, transformToFlip.localScale.y, transformToFlip.localScale.z);

        isLookingLeft = !isLookingLeft;
    }
}
