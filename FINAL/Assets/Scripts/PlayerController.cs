using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Particle;
using UnityEngine.SceneManagement;
using CASP.CameraManager;
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
    [SerializeField] GameObject player3D;
    [SerializeField] LayerMask layerMask;


    // *************************** Player Controller 2D ************************ 
    [Header("Player2D Controller")]
    [SerializeField] GameObject player2D;
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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1f, Color.white);
        if (Input.GetKeyDown(KeyCode.E) && !is2D)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f, layerMask))
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
                        CameraManager.instance.OpenCamera("2D 90 Degree Cam", 1, CameraEaseStates.Linear);
                        player3D.SetActive(false);
                        rb.freezeRotation = true; //divara deyende rotate problemini duzeldir
                        ParticleManager.instance.Play("Sparkle");
                        is2D = true;
                        player2D.SetActive(true);
                        Wall90 = true;
                        rb.constraints = RigidbodyConstraints.FreezePositionX;
                        sequence.Kill();



                    }));
                }
                else  //wall rotation 0;
                {
                    sequence.Append(transform.DORotate(new Vector3(0, 180, 0), 0.3f)).Insert(0.3f, transform.DOScale(new Vector3(1f, 1f, 0.04f), 0.05f))
                    .Insert(0.3f, transform.DOMoveZ(transform.position.z + 0.55f, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        CameraManager.instance.OpenCamera("2D Cam", 1, CameraEaseStates.Linear);
                        rb.freezeRotation = true;
                        // rb.constraints = RigidbodyConstraints.FreezePositionZ;
                        player3D.SetActive(false);
                        is2D = true;
                        player2D.SetActive(true);
                        ParticleManager.instance.Play("Sparkle");
                        Wall90 = false;
                        sequence.Kill();

                    }));
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.E) && is2D)
        {
            if (Wall90)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveX(transform.position.x - 5f, 0.3f))
                .Insert(0.3f, transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f)).OnComplete(() =>
                {
                    CameraManager.instance.OpenCamera("3D Cam", 1, CameraEaseStates.Linear);

                    Wall90 = false;
                    player2D.SetActive(false);
                    is2D = false;
                    // rb.freezeRotation = false;
                    lockMovement = false;
                    rb.constraints = RigidbodyConstraints.None;

                    player3D.SetActive(true);
                    Angle = 1f;
                    speed = 5f;
                    sequence.Kill();

                });
            }
            else
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveZ(transform.position.z - 1f, 0.3f)).Insert(0.3f, transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f)).OnComplete(() =>
                {
                    CameraManager.instance.OpenCamera("3D Cam", 1, CameraEaseStates.Linear);
                    Wall90 = false;
                    player2D.SetActive(false);
                    is2D = false;
                    rb.freezeRotation = false;
                    lockMovement = false;
                    player3D.SetActive(true);
                    Angle = 1f;
                    speed = 5f;
                    sequence.Kill();

                });
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }





    }
    private void FixedUpdate()
    {
        if (!lockMovement && !is2D) // 3D controller
        {
            UIManager.instance.StatusText("3D", "No Wall");

            UIManager.instance.HorizontalText(horizontal);  // STATUS CHECK
            UIManager.instance.VerticalText(vertical);


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
            UIManager.instance.StatusText("2D", "Straight Wall");

            if (Wall90)
            {

                UIManager.instance.StatusText("2D", "90 Degree Wall");

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
                horizontal2D = Input.GetAxis("Horizontal"); //0 derece divarda gezirik
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
    public void FlipX()
    {
        Vector3 currentScale = player2D.transform.localScale;
        currentScale.x *= -1;
        player2D.transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}