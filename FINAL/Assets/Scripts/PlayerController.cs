using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] float speed;
    [SerializeField] float JumpForce = 5f;
    float horizontal;
    float vertical;
    float SmoothTurnTime = 0.1f;
    float CurrentTurnAngle;
    Vector3 Direction;
    Rigidbody rb;
    Animator anim;
    bool isGround;
    bool changed2D = false;
    bool lockMovement = false;
    [SerializeField] GameObject playerModel;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            // anim.SetTrigger("Jump");
            isGround = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position + (Vector3.up * .1f), transform.forward, out RaycastHit hit, 1))
            {
                lockMovement = true;
                JumpForce = 0f;
                speed = 0f;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DORotate(new Vector3(0, 180, 0), 0.3f)).Insert(0.3f, transform.DOScale(new Vector3(1f, 1f, 0.1f), 0.01f)).Insert(0.3f, transform.DOMoveZ(transform.position.z + 0.5f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    ParticleManager.instance.PlaySparkle();
                    changed2D = true;


                }));
            }
        }
    }
    private void FixedUpdate()
    {
        if (!lockMovement)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Direction = new Vector3(horizontal, 0, vertical);
            if (Direction.magnitude > 0.01f)
            {
                float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
                float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref CurrentTurnAngle, SmoothTurnTime);
                transform.rotation = Quaternion.Euler(0, Angle, 0);
                rb.MovePosition(transform.position + (Direction * speed * Time.deltaTime));
            }
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Wall") && changed2D)
        {
            playerModel.SetActive(false);
        }
    }
}