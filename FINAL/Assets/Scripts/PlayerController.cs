using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Particle;

public class PlayerController : MonoBehaviour
{
    [Header("Controller")] // *************************** Player Controller 3D ************************ 
    [SerializeField] float speed;
    [SerializeField] float JumpForce = 5f;
    float horizontal;
    float vertical;
    float SmoothTurnTime = 0.1f;
    float CurrentTurnAngle;
    float Angle;
    Vector3 Direction;
    Rigidbody rb;
    Animator anim;
    bool isGround;
    bool lockMovement = false;
    [SerializeField] GameObject playerModel;
    [SerializeField] LayerMask layerMask;


    // *************************** Player Controller 3D ************************ 
    [Header("Player2D Controller")]
    [SerializeField] GameObject Player2D;
    float horizontal2D;
    float vertical2D;
    [SerializeField] float speed2D;
    [SerializeField] float JumpForce2D;
    private bool facingRight = true;
    bool is2D = false;
    bool Wall90;



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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        if (Input.GetKeyDown(KeyCode.E) && !is2D)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, layerMask))
            {

                lockMovement = true;
                Angle = 0f;
                // JumpForce = 0f;
                speed = 0f;


                Sequence sequence = DOTween.Sequence();
                if (hit.transform.rotation.y > 0f)
                {
                    sequence.Append(transform.DORotate(new Vector3(0f, 180f + hit.transform.eulerAngles.y, 0f), 0.4f)).Insert(0.4f, transform.DOScale(new Vector3(1f, 1f, 0.1f), 0.1f))
                    .Insert(0.4f, transform.DOMoveX(transform.position.x + 0.55f, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        rb.freezeRotation = true; //divara deyende rotate problemini duzeldir
                        rb.constraints = RigidbodyConstraints.FreezePositionX;

                        ParticleManager.instance.Play("Sparkle");

                        is2D = true;
                        Player2D.SetActive(true);
                        sequence.Kill();
                        Wall90 = true;
                        Debug.Log("NOT 0 degree wall");


                    }));
                }
                else  //wall rotation 0;
                {
                    sequence.Append(transform.DORotate(new Vector3(0, 180, 0), 0.3f)).Insert(0.3f, transform.DOScale(new Vector3(1f, 1f, 0.1f), 0.01f))
                    .Insert(0.3f, transform.DOMoveZ(transform.position.z + 0.55f, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        rb.freezeRotation = true;
                        // rb.constraints = RigidbodyConstraints.FreezePositionZ;

                        is2D = true;
                        Player2D.SetActive(true);
                        sequence.Kill();
                        ParticleManager.instance.Play("Sparkle");
                        Debug.Log("0 degree wall");

                    }));
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!lockMovement) // 3D controller
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Direction = new Vector3(horizontal, 0, vertical);
            if (Direction.magnitude > 0.01f)
            {
                float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
                Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref CurrentTurnAngle, SmoothTurnTime);
                transform.rotation = Quaternion.Euler(0, Angle, 0);
                rb.MovePosition(transform.position + (Direction * speed * Time.deltaTime));
            }
        }
        if (is2D)
        {
            if (Wall90)
            {
                horizontal2D = Input.GetAxis("Horizontal");
                transform.position += new Vector3(0, 0, (horizontal2D * speed2D) * -1) * Time.deltaTime;

                if (horizontal2D > 0 && facingRight)
                {
                    FlipX();
                }
                if (horizontal2D < 0 && !facingRight)
                {
                    FlipX();
                }
            }
            else
            {
                horizontal2D = Input.GetAxis("Horizontal");
                transform.position += new Vector3(horizontal2D * speed2D, 0, 0) * Time.deltaTime;

                if (horizontal2D > 0 && facingRight)
                {
                    FlipX();
                }
                if (horizontal2D < 0 && !facingRight)
                {
                    FlipX();
                }
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
        if (other.transform.CompareTag("Wall") && is2D)
        {
            playerModel.SetActive(false);
        }
    }

    public void FlipX()
    {
        Vector3 currentScale = Player2D.transform.localScale;
        currentScale.x *= -1;
        Player2D.transform.localScale = currentScale;
        facingRight = !facingRight;
        Debug.Log("flip x");

    }
}