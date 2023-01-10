using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Particle;
using UnityEngine.SceneManagement;
using CASP.CameraManager;
using CASP.SoundManager;
public class PlayerController : MonoBehaviour
{
    #region 3D Global
    [Header("Controller")] // *************************** Player Controller 3D ************************ 
    [SerializeField] float speed;
    [SerializeField] float moveObjectspeed;
    [SerializeField] float JumpForce;
    float horizontal;
    float vertical;
    float SmoothTurnTime = 0.04f;
    float CurrentTurnAngle;
    float Angle;
    Vector3 Direction;
    Rigidbody rb;
    [SerializeField] Animator anim;
    bool isGround;
    [HideInInspector]
    public bool lockMovement = false;
    [SerializeField] GameObject player3D;
    [SerializeField] LayerMask layerMask; //Raycast Layer
    [SerializeField] Transform DetectTransform;
    [SerializeField] Transform RaycastTransform;
    [SerializeField] private float DetectionRange;
    [SerializeField] LayerMask puzzleLayer; //OverlapSphere Layer
    Collider[] colliders;
    bool moveObjectMode = false;
    int EpressCount = 0;

    #endregion

    #region 2D  Global
    // *************************** Player Controller 2D ************************ 
    [Header("Player2D Controller")]
    [SerializeField] GameObject player2D;
    [SerializeField] GameObject player2Elevator;

    float horizontal2D;
    float vertical2D;
    [SerializeField] float speed2D;
    private bool facingRight = false;
    bool is2D = false;
    bool Wall90;
    #endregion
    [SerializeField] Animator anim2D;
    bool isMoving2D = false;

    // **********************************  Puzzle *********************************
    bool LeverBladeUsed = false;
    bool LeverWallUsed = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.6f, 0f, 0f, 0.2f);
        Gizmos.DrawSphere(DetectTransform.position, DetectionRange);

    }
    void Update()
    {

        colliders = Physics.OverlapSphere(DetectTransform.position, DetectionRange, puzzleLayer);
        foreach (var hit in colliders)
        {
            #region Hold&Move Object
            if (Input.GetKeyDown(KeyCode.E) && hit.CompareTag("Stone"))
            {
                anim.SetFloat("Run", 0f);
                EpressCount++;
                switch (EpressCount)
                {
                    default:
                        break;
                    case 1:
                        anim.SetBool("MoveIdle", true);
                        JumpForce = 0f;
                        moveObjectMode = true;
                        UIManager.instance.StatusText("3D, Move Object ", " ");
                        hit.transform.SetParent(transform);
                        break;
                    case 2:
                        anim.SetBool("MoveIdle", false);
                        anim.SetBool("Move", false);
                        JumpForce = 3f;
                        moveObjectMode = false;
                        UIManager.instance.StatusText("3D,No Move Object ", " ");
                        hit.transform.parent = null;
                        EpressCount = 0;
                        break;
                }
            }
            #endregion

            #region Lever
            if (Input.GetKeyDown(KeyCode.E) && hit.CompareTag("LeverBlade") && !LeverBladeUsed)
            {
                Sequence sequenceSound = DOTween.Sequence();
                sequenceSound.AppendInterval(2f).OnComplete(() =>
                {
                    SoundManager.instance.Play("Lever", false);
                });
                JumpForce = 0;
                rb.isKinematic = true;
                anim.SetFloat("Run", 0f);
                LeverBladeUsed = true;
                lockMovement = true;
                Sequence PlayerTransformCorrection = DOTween.Sequence();
                transform.DOMove(new Vector3(hit.transform.position.x - 1.04f, 0f, hit.transform.position.z), .7f);
                PlayerTransformCorrection.Append(transform.DORotate(new Vector3(0, 90, 0), 0.7f));
                anim.SetTrigger("Lever");
                Animator animLever = hit.GetComponent<Animator>();
                animLever.SetBool("On", true);
                PlayerTransformCorrection.AppendInterval(3.5f).OnComplete(() =>
                {
                    rb.isKinematic = false;
                    JumpForce = 3f;
                });
                PuzzleManager.instance.BladeKill();
            }
            #endregion

            #region LeverWall
            if (Input.GetKeyDown(KeyCode.E) && hit.CompareTag("LeverWall") && !LeverWallUsed)
            {
                Sequence sequenceSound = DOTween.Sequence();
                sequenceSound.AppendInterval(2f).OnComplete(() =>
                {
                    SoundManager.instance.Play("Lever", false);
                });
                JumpForce = 0;
                rb.isKinematic = true;
                anim.SetFloat("Run", 0f);
                LeverWallUsed = true;
                lockMovement = true;
                Sequence PlayerTransformCorrection = DOTween.Sequence();
                transform.DOMove(new Vector3(hit.transform.position.x - 1.04f, 0f, hit.transform.position.z), .7f);
                PlayerTransformCorrection.Append(transform.DORotate(new Vector3(0, 90, 0), 0.7f));
                SoundManager.instance.Play("Lever", false);
                anim.SetTrigger("Lever");
                Animator animLever = hit.GetComponent<Animator>();
                animLever.SetBool("On", true);
                PlayerTransformCorrection.AppendInterval(3.5f).OnComplete(() =>
               {
                   rb.isKinematic = false;
                   JumpForce = 3f;

               });
                PuzzleManager.instance.Wallin();
            }
            #endregion

            #region StoneBuildingButton

            if (Input.GetKeyDown(KeyCode.E) && hit.CompareTag("StoneButton"))
            {
                GameObject buttonGo = hit.gameObject;
                PuzzleManager.instance.ButtonPress(buttonGo);
            }

            #endregion

            #region Collectable
            if (hit.transform.CompareTag("Collectable"))
            {
                hit.GetComponent<BoxCollider>().enabled = false;
                hit.transform.DOJump(transform.position, 2, 1, 0.2f).OnComplete(() =>
                {
                    hit.gameObject.SetActive(false);
                });
            }
            #endregion

        }

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            StartCoroutine(GroundTrue());
            isGround = false;
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            anim.SetBool("Jump", true);
        }
        #endregion

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2f, Color.white);

        #region Enter2D
        if (Input.GetKeyDown(KeyCode.E) && !is2D)
        {
            RaycastHit hit;
            if (Physics.Raycast(RaycastTransform.position, RaycastTransform.TransformDirection(Vector3.forward), out hit, 1f, layerMask))
            {
                if (hit.transform.parent == null)
                {
                    transform.SetParent(hit.transform);
                }
                else
                {
                    transform.SetParent(hit.transform.parent);
                }


                lockMovement = true;
                Angle = 0f;
                JumpForce = 0f;
                speed = 0f;


                Sequence sequence = DOTween.Sequence();
                if (hit.transform.rotation.y > 0f) //                                            90 derece divar
                {
                    sequence.Append(transform.DORotate(new Vector3(0f, 180f + hit.transform.eulerAngles.y, 0f), 0.4f)).Insert(0.4f, transform.DOScale(new Vector3(1f, 1f, 0.1f), 0.65f))
                    .Insert(0.7f, transform.DOMoveX(transform.position.x + 0.55f, 0.6f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        CameraManager.instance.OpenCamera("2D 90 Degree Cam", 1, CameraEaseStates.Linear);
                        player3D.SetActive(false);
                        rb.isKinematic = true;
                        ParticleManager.instance.Play("Sparkle");
                        is2D = true;
                        player2D.SetActive(true);
                        Wall90 = true;
                        rb.constraints = RigidbodyConstraints.FreezePositionX;
                        rb.freezeRotation = true;
                        sequence.Kill();
                        FeedbackManager.Instance.ModeChanged.PlayFeedbacks();
                    }));
                }
                else  //                                                                          wall 0;
                {
                    sequence.Append(transform.DORotate(new Vector3(0, 180, 0), 0.3f)).Insert(0.3f, transform.DOScale(new Vector3(transform.localScale.x, transform.localScale.y, 0.04f), 0.4f))
                    .Insert(0.5f, transform.DOMoveZ(transform.position.z + 0.55f, 0.5f).SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        CameraManager.instance.OpenCamera("2D Cam", 1, CameraEaseStates.Linear);
                        rb.freezeRotation = true;
                        rb.isKinematic = true;
                        player3D.SetActive(false);
                        is2D = true;
                        ParticleManager.instance.Play("Sparkle");
                        Wall90 = false;
                        sequence.Kill();
                        FeedbackManager.Instance.ModeChanged.PlayFeedbacks();
                        if (hit.transform.CompareTag("Elevator"))
                        {
                            PuzzleManager.instance.ElevatorwithCharacter();
                        }
                        else
                        {
                            player2D.SetActive(true);
                        }
                    }));
                }

                if (Input.GetKeyDown(KeyCode.E) && hit.transform.CompareTag("Elevator") && !PuzzleManager.instance.elUp)
                {
                    PuzzleManager.instance.ElevatorUp();

                }
                else
                {
                    PuzzleManager.instance.ElevatorDown();
                }
            }
        }
        #endregion

        #region Exit 2D
        if (Input.GetKeyDown(KeyCode.E) && is2D && !PuzzleManager.instance.isElevatorMoving)
        {
            isGround = true;
            transform.parent = null;

            if (Wall90)
            {
                player2D.SetActive(false);
                player3D.SetActive(true);
                CameraManager.instance.OpenCamera("3D Cam", 1, CameraEaseStates.Linear);

                Sequence sequence = DOTween.Sequence();                                               // Exit wall 90
                sequence.Append(transform.DOMoveX(transform.position.x - 1f, 0.3f))
                .Insert(0.2f, transform.DOScale(new Vector3(1f, 1f, 1f), 0.4f)).OnComplete(() =>
                {

                    Wall90 = false;
                    PuzzleManager.instance.ElevatorwithoutCharacter();


                    is2D = false;
                    rb.isKinematic = false;
                    lockMovement = false;
                    rb.constraints = RigidbodyConstraints.None;
                    rb.freezeRotation = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

                    Angle = 1f;
                    speed = 3.2f;
                    JumpForce = 3.6f;
                    sequence.Kill();

                });
            }
            else                                                                                //Exit wall 0
            {
                player2D.SetActive(false);
                player3D.SetActive(true);
                PuzzleManager.instance.ElevatorwithoutCharacter();

                CameraManager.instance.OpenCamera("3D Cam", 1, CameraEaseStates.Linear);

                //sequence.Append(transform.DOMoveZ(transform.position.z - 1f, 0.5f)).Insert(0.5f, transform.DOScale(new Vector3(1f, 1f, 1f), 0.05f)).OnComplete(() =>

                Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.8f)).Join(transform.DOMoveZ(transform.position.z - 1f, 0.3f)).OnComplete(() =>
               {
                   Wall90 = false;


                   is2D = false;
                   rb.constraints = RigidbodyConstraints.None;
                   rb.freezeRotation = false;
                   rb.isKinematic = false;
                   rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

                   JumpForce = 3.7f;
                   lockMovement = false;
                   Angle = 1f;
                   speed = 3.2f;
                   sequence.Kill();

               });
            }

        }
        #endregion

    }

    private void FixedUpdate()
    {
        #region 3D controller
        if (!lockMovement && !is2D && !moveObjectMode) // 3D controller
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
                rb.MovePosition(transform.position + (Direction.normalized * speed * Time.deltaTime));
                anim.SetFloat("Run", Direction.magnitude);
            }
        }


        if (moveObjectMode)
        {

            anim.SetBool("MoveIdle", true);
            anim.SetBool("Move", false);
            horizontal = -Input.GetAxis("Horizontal");
            vertical = -Input.GetAxis("Vertical");
            Direction = new Vector3(horizontal, 0, vertical);
            if (Direction.magnitude > 0.01f)
            {
                anim.SetBool("Move", true);
                anim.SetBool("MoveIdle", false);

                SoundManager.instance.Play("StoneDrag", true);
                float TargetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
                Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref CurrentTurnAngle, 0.8f); //smooth turn time 0.5f
                transform.rotation = Quaternion.Euler(0, Angle, 0);
                rb.MovePosition(transform.position - (Direction * moveObjectspeed * Time.deltaTime));
            }
            else
            {
                anim.SetBool("Move", false);
                SoundManager.instance.Stop("StoneDrag");
            }
        }
        #endregion
        #region 2D Controller

        if (is2D)
        {
            UIManager.instance.StatusText("2D", "Straight Wall");
            if (Wall90)
            {
                UIManager.instance.StatusText("2D", "90 Degree Wall");
                // float clampZ = Mathf.Clamp(transform.position.z,-9.946901f, 1.046956f);

                horizontal2D = Input.GetAxis("Horizontal");
                transform.position += new Vector3(0, 0, (horizontal2D * speed2D) * -1) * Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, -11.5f, 1.5f));
                if (horizontal2D > 0 && facingRight)
                {
                    FlipX();
                }
                if (horizontal2D < 0 && !facingRight)
                {
                    FlipX();
                }
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    isMoving2D = true;
                }
                else
                {
                    isMoving2D = false;
                }
                if (isMoving2D)
                {
                    anim2D.SetBool("Run", true);
                }
                else
                {
                    anim2D.SetBool("Run", false);
                }

            }
            // else
            // { //wall 0

            //     // horizontal2D = Input.GetAxis("Horizontal"); //0 derece divarda gezirik
            //     // transform.position += new Vector3(horizontal2D * speed2D, 0, 0) * Time.deltaTime;

            //     if (horizontal2D > 0 && facingRight)
            //     {
            //         FlipX();
            //     }
            //     if (horizontal2D < 0 && !facingRight)
            //     {
            //         FlipX();
            //     }
            // }

        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGround = true;
            anim.SetBool("Jump", false);
            SoundManager.instance.Play("Land", false);
            ParticleManager.instance.Play("Land");
        }

        if (other.transform.CompareTag("Collectable"))
        {
            PuzzleManager.instance.ItemUnlocked(other.gameObject);
            SoundManager.instance.Play("Collect", false);
            other.transform.DOJump(transform.position, 1, 1, 0.4f).OnComplete(() =>
             {
                 other.gameObject.SetActive(false);
                 if (other.gameObject.name == "pisa")
                 {
                     ParticleManager.instance.Play("Portal");
                 }
             });
        }

        if (other.transform.CompareTag("Ritual"))
        {
            PuzzleManager.instance.Ritual();
        }
    }
    public void FlipX()
    {
        Vector3 currentScale = player2D.transform.localScale;
        currentScale.x *= -1;
        player2D.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    IEnumerator GroundTrue()
    {
        yield return new WaitForSeconds(.5f);
        isGround = true;
        anim.SetBool("Jump", false);
    }


}