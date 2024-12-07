using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanoidLandController : MonoBehaviour
{
    public static HumanoidLandController Instance { get; private set; }

    [SerializeField] public HumanoidLandInput input;
    [SerializeField] Camera playerCamera;

    [SerializeField] public Slider DashSlider;
    [SerializeField] public Slider BoostSlider;
    [SerializeField] public Slider HandSlider;

    [SerializeField] public Image DashImg;
    [SerializeField] public Image BoostImg;
    [SerializeField] public Image HandImg;

    Rigidbody rigidbody;
    CapsuleCollider capsuleCollider = null;

    [SerializeField] Vector3 playerMoveInput = Vector3.zero;


    [Header("Movement")]
    [SerializeField] public bool isWalking = false;
    [SerializeField] public bool isRunning = false;
    [SerializeField] float movementMultiplier = 32.0f;
    [SerializeField] float playerNotGroundedMovementMultiplier = 1.125f;
    [SerializeField] float rotationSpeedMultiplier = 900.0f;
    [SerializeField] float runSpeedMultiplier = 2.5f;

    [Header("Ground Check")]
    [SerializeField] public bool playerIsGrounded = true;
    [SerializeField][Range(0f, 1.8f)] float groundCheckRadiusMultiplier = 1.6f;
    [SerializeField][Range(-0.95f, 1.05f)] float groundCheckDistance = 0.05f;
    RaycastHit groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float gravityFallCurrent = -10.0f;
    [SerializeField] float gravityFallMin = -10.0f;
    [SerializeField] float gravityFallMax = -175.0f;
    [SerializeField] float gravityFallMaxWhenFalling = -300.0f;
    [SerializeField][Range(-1.0f, -35.0f)] float gravityFallIncrementAmount = -2.5f;
    [SerializeField] float gravityFallIncrementTime = -0.05f;
    [SerializeField] float PlayerFallTimer = 0.0f;
    [SerializeField] float gravityGrounded = 0.0f;
    [SerializeField] float maxSlopeAngle = 50.0f;

    [Header("Jumping")]
    [SerializeField] float initialJumpForce = 1000.0f;
    [SerializeField] float continualJumpForceMultiplier = 0.05f;
    [SerializeField] float jumpTime = 0.175f;
    [SerializeField] float jumpTimeCounter = 0.0f;
    [SerializeField] float coyoteTime = 0.4f;
    [SerializeField] public float coyoteTimeCounter = 0.0f;
    [SerializeField] float jumpBufferTime = 0.8f;
    [SerializeField] float jumpBufferTimeCounter = 0.0f;
    [SerializeField] public bool playerIsJumping = false;
    [SerializeField] bool JumpWasPressedLastFrame = false;
    [SerializeField] public bool isDoubleJumping = false;
    [SerializeField] float doubleJumpDelay = 1.0f; 
    [SerializeField] float doubleJumpForce = 3f; 
    [SerializeField] float doubleJumpSpeedMultiplier = 1.25f; 
    [SerializeField] public bool canDoubleJump = false; 
    [SerializeField] float doubleJumpTimer = 0f;
    [SerializeField] public bool wasAirborne = false;
    [SerializeField] public bool x = false;
    [SerializeField] public float GroundedAfterLandingTimer = 0.02f;


    [Header("Extra Movement")]

    [Header("Dash")]
    [SerializeField] public bool isDashing = false;
    [SerializeField] public bool startedDash = false;
    [SerializeField] public bool canDash = true;
    [SerializeField] float DashForce = 2500.0f;
    [SerializeField] float DashDelay = 1.5f;
    [SerializeField] float DashTimer = 0f;
    [SerializeField] float DashTime = 0.4f;
    [SerializeField] float DashTimeMax = 0.0f;

    [Header("Boost")]
    [SerializeField] public bool isBoosting = false;
    [SerializeField] public bool startedBoost = false;
    [SerializeField] public bool canBoost = true;
    [SerializeField] float BoostForce = 2500.0f;
    [SerializeField] float BoostDelay = 1.5f;
    [SerializeField] float BoostTimer = 0f;
    [SerializeField] float BoostTime = 0.4f;
    [SerializeField] float BoostTimeMax = 0.0f;

    [Header("Place Hand")]
    [SerializeField] public bool canPlaceHand = true;
    [SerializeField] public bool handIsActive = false;
    [SerializeField] public bool handPlaceConfirm = false;
    [SerializeField] public bool isSlomo = false;
    [SerializeField] Vector3 handInitialPosition = Vector3.zero;
    [SerializeField] float targetTimeSpeed = 0.02f;
    [SerializeField] float handPlaceRadius = 10.0f;
    [SerializeField] float handPlaceMoveSpeed = 10.0f;
    //[SerializeField] float handHorizontalSpeed = 10.0f;
    [SerializeField] float handPlaceRecharge = 0.0f;
    //[SerializeField] float handSpawnTime = 0.0f;
    [SerializeField] float handLifeTime = 5.0f;
    [SerializeField] bool canCountLife = false;
    [SerializeField] bool canCountRecharge = false;
    public GameObject ghostHand;
    public GameObject GodsHand;
    private GameObject placedHand;
    private Transform playerTransform;
    [SerializeField] public bool canPlaceHandWhenGrounded = true;
    [SerializeField] public bool standingOnHand = false;

    [Header("Misc")]
    [SerializeField] public bool isFalling = false;
    [SerializeField] public float defFixedUpdateSpeed;
    private Camera mainCamera;

    [Header("Particles")]
    public ParticleSystem Trail;
    public ParticleSystem Poof;
    public ParticleSystem Dash;
    public ParticleSystem Boost;
    public ParticleSystem Falling;


    private void Awake()
    {
        Instance = this;
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        defFixedUpdateSpeed = Time.fixedDeltaTime;
        playerTransform = transform;
        mainCamera = Camera.main;
        
    }

    private void Start()
    {
        TimerController.instance.BeginTimer();
    }

    private void FixedUpdate()
    {
        SetBarValues();
        RotatePlayer();

        playerMoveInput = GetMoveInput();
        playerIsGrounded = PlayerGroundCheck();

        playerMoveInput = PlayerMove();
        playerMoveInput = PlayerSlope();
        playerMoveInput = PlayerRun();

        playerMoveInput.y = PlayerFallGravity();
        playerMoveInput.y = PlayerJump();

        playerMoveInput *= rigidbody.mass;

        PlayerDash();
        PlayerBoost();
        PlayerMiscAtributes();
        HandAblility();

        if (handIsActive)
        {
            MoveHandWithPlayer();
            MoveHandWithCursor();
            MoveHandWithScroll();
        }

        if (canPlaceHandWhenGrounded && playerIsGrounded && !standingOnHand)
        {
            canPlaceHandWhenGrounded = false;
            canPlaceHand = true;
        }

        rigidbody.AddForce(playerMoveInput, ForceMode.Force);
    }


    private Vector3 GetMoveInput()
    {
        Vector3 moveInput = new Vector3(input.MoveInput.x, 0.0f, input.MoveInput.y);
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 desiredMoveDirection = cameraForward * moveInput.z + cameraRight * moveInput.x;
        return desiredMoveDirection;
    }

    private void PlayerDash()
    {
        if ((playerIsGrounded || standingOnHand) && !isDashing && DashTimer <= 0.0f && !isBoosting && !isSlomo)
        {
            canDash = true;
        }

        Vector3 moveDirection = (input.MoveIsPressed) ? GetMoveInput() : transform.forward;
        moveDirection.y = 0.1f;

        if (input.DashIsPressed && canDash && DashTimer <= 0.0f && !isBoosting && !isSlomo)
        {
            Vector3 forceDirection = moveDirection.normalized;

            rigidbody.AddForce(forceDirection * DashForce, ForceMode.Impulse);

            startedDash = true;
            isDashing = true;
            canDash = false;
            DashTimer = DashDelay;
            DashTimeMax = DashTime;
        }
        if (DashTimeMax > 0f)
        {
            DashTimeMax -= Time.fixedDeltaTime;
        }
        else
        {
            isDashing = false;
        }
        if (DashTimer > 0f)
        {
            DashTimer -= Time.fixedDeltaTime;
            if (DashTimer < 1.0f)
            {
                startedDash = false;
            }
        }
    }


    private void PlayerBoost()
    {
        if ((playerIsGrounded || standingOnHand) && !isBoosting && BoostTimer <= 0.0f && !isDashing && !isSlomo)
        {
            canBoost = true;
        }

        Vector3 moveDirection = transform.up;

        if (input.BoostIsPressed && canBoost && BoostTimer <= 0.0f && !isDashing && !isSlomo)
        {
            Vector3 forceDirection = moveDirection.normalized;

            rigidbody.AddForce(forceDirection * BoostForce, ForceMode.Impulse);

            startedBoost = true;
            isBoosting = true;
            canBoost = false;
            BoostTimer = BoostDelay;
            BoostTimeMax = BoostTime;
        }
        if (BoostTimeMax > 0f)
        {
            BoostTimeMax -= Time.fixedDeltaTime;
        }
        else
        {
            isBoosting = false;
        }

        if (BoostTimer > 0f)
        {
            BoostTimer -= Time.fixedDeltaTime;
            if (BoostTimer < 1.0f)
            {
                startedBoost = false;
            }
        }
    }

    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = capsuleCollider.radius * groundCheckRadiusMultiplier;
        float sphereCastTravelDistance = capsuleCollider.bounds.extents.y - sphereCastRadius + groundCheckDistance;
        Vector3 start = rigidbody.position + new Vector3(0, 1f, 0);

        if (Physics.SphereCast(start, sphereCastRadius, Vector3.down, out groundCheckHit, sphereCastTravelDistance) && groundCheckHit.collider.tag != "hand") 
        {
            return true; 
        }
        else if (Physics.SphereCast(start, sphereCastRadius, Vector3.down, out groundCheckHit, sphereCastTravelDistance) && groundCheckHit.collider.tag == "hand")
        {
            standingOnHand = true;
            return false;
        }
        standingOnHand = false;
        return false;
    }

    private Vector3 PlayerMove()
    {
        if (!playerIsGrounded && !standingOnHand)
        {
            if (isDoubleJumping)
            {
                return playerMoveInput * movementMultiplier * doubleJumpSpeedMultiplier;
            }
            else
            {
                return playerMoveInput * movementMultiplier * playerNotGroundedMovementMultiplier;
            }
        }
        else
        {
            return playerMoveInput * movementMultiplier;
        }
    }

    private Vector3 PlayerSlope()
    {
        Vector3 calculatedPlayerMovement = playerMoveInput;

        if (playerIsGrounded || standingOnHand)
        {
            Vector3 localGroundCheckHitNormal = rigidbody.transform.InverseTransformDirection(groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, rigidbody.transform.up);
            if (groundSlopeAngle == 0.0f)
            {
                if (input.MoveIsPressed)
                {
                    RaycastHit rayHit;
                    float rayHeightFromGround = 0.1f;
                    float rayCalculatedRayHeight = rigidbody.position.y - capsuleCollider.bounds.extents.y + rayHeightFromGround;
                    Vector3 rayOrigin = new Vector3(rigidbody.position.x, rayCalculatedRayHeight, rigidbody.position.z);
                    if (Physics.Raycast(rayOrigin, rigidbody.transform.TransformDirection(calculatedPlayerMovement), out rayHit, 0.5f))
                    {
                        if (Vector3.Angle(rayHit.normal, rigidbody.transform.up) > maxSlopeAngle)
                        {
                            calculatedPlayerMovement.y = -movementMultiplier;
                        }
                    }
                    Debug.DrawRay(rayOrigin, rigidbody.transform.TransformDirection(calculatedPlayerMovement), Color.green, 1.0f);
                }
                if (calculatedPlayerMovement.y == 0.0f)
                {
                    calculatedPlayerMovement.y = gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(rigidbody.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, rigidbody.transform.up) - 90.0f;
                calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / 90.0f);

                if (groundSlopeAngle > maxSlopeAngle)
                {
                    if (input.MoveIsPressed)
                    {
                        calculatedPlayerMovement.y += gravityGrounded;
                    }
                }
                else if (groundSlopeAngle < (maxSlopeAngle / 2))
                {
                    calculatedPlayerMovement.y = 0.0f;
                }
                else
                {
                    float calculatedSlopeGravity = groundSlopeAngle * -0.2f;
                    if (calculatedSlopeGravity < calculatedPlayerMovement.y)
                    {
                        calculatedPlayerMovement.y = calculatedSlopeGravity;
                    }
                }
            }
        }
        return calculatedPlayerMovement;
    }

    private Vector3 PlayerRun()
    {
        Vector3 calculatedPlayerRunSpeed = playerMoveInput;
        if (input.MoveIsPressed && input.RunIsPressed)
        {
            calculatedPlayerRunSpeed *= runSpeedMultiplier;
        }
        return calculatedPlayerRunSpeed;
    }

    private float PlayerFallGravity()
    {
        float gravity = playerMoveInput.y;
        if (playerIsGrounded || standingOnHand)
        {
            gravityFallCurrent = gravityFallMin;
        }
        else
        {
            PlayerFallTimer -= Time.fixedDeltaTime;
            if (PlayerFallTimer < 0.0f)
            {
                if (isFalling)
                {
                    if (gravityFallCurrent > gravityFallMaxWhenFalling)
                    {
                        gravityFallCurrent += gravityFallIncrementAmount;
                    }
                }
                else if (gravityFallCurrent > gravityFallMax)
                {
                    gravityFallCurrent += gravityFallIncrementAmount;
                }
                PlayerFallTimer = gravityFallIncrementTime;
                gravity = gravityFallCurrent;
            }
        }
        return gravity;
    }

    private float PlayerJump()
    {
        float calculatedJumpInput = playerMoveInput.y;

        SetJumpTimeCounter();
        SetCoyoteTimeCounter();
        SetJumpBufferTimeCounter();

        if (jumpBufferTimeCounter > 0.0f && !playerIsJumping && coyoteTimeCounter > 0.0f && !isDashing && !isBoosting && !isSlomo)
        {
            if (Vector3.Angle(rigidbody.transform.InverseTransformDirection(groundCheckHit.normal), rigidbody.transform.up) < maxSlopeAngle)
            {
                calculatedJumpInput = initialJumpForce;
                playerIsJumping = true;
                isDoubleJumping = false; 
                doubleJumpTimer = doubleJumpDelay; 
                canDoubleJump = false;
                jumpBufferTimeCounter = 0.0f;
                coyoteTimeCounter = 0.0f;
            }
        }
        else if (input.JumpIsPressed && playerIsJumping && !playerIsGrounded && !standingOnHand && jumpTimeCounter > 0.0f && !isDashing && !isBoosting)
        {
            calculatedJumpInput = initialJumpForce * continualJumpForceMultiplier;
        }
        else if (playerIsJumping && !playerIsGrounded && !standingOnHand  && input.JumpIsPressed && canDoubleJump && !isDashing && !isBoosting && !isSlomo)
        {
            isDoubleJumping = true;
            calculatedJumpInput = initialJumpForce * doubleJumpForce;
            canDoubleJump = false; 
            doubleJumpTimer = doubleJumpDelay;
        }
        else if (playerIsJumping && (playerIsGrounded || standingOnHand))
        {
            playerIsJumping = false;
            isDoubleJumping = false;
            canDoubleJump = false;
            doubleJumpTimer = doubleJumpDelay;
        }

        if (doubleJumpTimer > 0f && !isDoubleJumping)
        {
            doubleJumpTimer -= Time.fixedDeltaTime;
        }
        else if (doubleJumpTimer <= 0f)
        {
            canDoubleJump = true;
        }

        return calculatedJumpInput;
    }


private void SetJumpTimeCounter()
    {
        if (playerIsJumping && !playerIsGrounded && !standingOnHand)
        {
            jumpTimeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            jumpTimeCounter = jumpTime;
        }
    }

    private void SetCoyoteTimeCounter()
    {
        if (playerIsGrounded || standingOnHand)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void SetJumpBufferTimeCounter()
    {
        if (!JumpWasPressedLastFrame && input.JumpIsPressed)
        {
           
            if (playerIsGrounded || isDoubleJumping || standingOnHand)
            {
                jumpBufferTimeCounter = jumpBufferTime;
            }
        }
        else if (jumpBufferTimeCounter > 0.0f)
        {
            jumpBufferTimeCounter -= Time.fixedDeltaTime;
        }
        JumpWasPressedLastFrame = input.JumpIsPressed;
    }

    private void HandAblility()
    {
        if (canPlaceHand && input.PlaceHandIsPressed)
        {
            canPlaceHand = false;
            isSlomo = true;

            Time.timeScale = targetTimeSpeed;
            Time.fixedDeltaTime = 0.001f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SpawnHand();
        }

        if (isSlomo && input.ConfirmPlaceIsPressed)
        {
            FinalizePlacement();
        }

        if (canCountLife)
        {
            if (handLifeTime > 0.0f)
            {
                handLifeTime -= Time.fixedDeltaTime;
            }
            else
            {
                canCountLife = false;
                handLifeTime = 5.0f;
                Destroy(placedHand);


                canCountRecharge = true;
                handPlaceRecharge = 0.0f;
                canPlaceHand = false;
            }
        }

        if (canCountRecharge)
        {
            if (handPlaceRecharge < 10.0f)
            {
                handPlaceRecharge += Time.fixedDeltaTime;
            }
            else
            {
                canPlaceHandWhenGrounded = true;
                canCountRecharge = false;
                handPlaceRecharge = 0.0f;
            }
        }

    }

    private void SpawnHand()
    {
        Vector3 handPosition = playerTransform.position + Vector3.down * 1.0f;
        placedHand = Instantiate(ghostHand, handPosition, Quaternion.identity);
        handIsActive = true;

        UpdateHandRotation();
    }

    private void MoveHandWithPlayer()
    {
        Vector3 offset = placedHand.transform.position - playerTransform.position;
        placedHand.transform.position = playerTransform.position + offset;

        UpdateHandRotation();
    }

    private void MoveHandWithCursor()
    {
        Vector3 mousePosition = Input.mousePosition;

        float normalizedX = mousePosition.x / Screen.width;
        float normalizedY = mousePosition.y / Screen.height;

        float radius = handPlaceRadius;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        Vector3 newHandPosition = playerTransform.position + (right * ((normalizedX - 0.5f) * 2 * radius)) +
                                                            (forward * ((normalizedY - 0.5f) * 2 * radius));

        if (Vector3.Distance(playerTransform.position, newHandPosition) > radius)
        {
            Vector3 direction = (newHandPosition - playerTransform.position).normalized;
            newHandPosition = playerTransform.position + direction * radius;
        }

        placedHand.transform.position = new Vector3(newHandPosition.x, placedHand.transform.position.y, newHandPosition.z);

        Vector3 playerEulerAngles = playerTransform.eulerAngles;
        placedHand.transform.rotation = Quaternion.Euler(0, playerEulerAngles.y, 0);
    }


    private void MoveHandWithScroll()
    {
        if (handIsActive)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0)
            {
                Vector3 newHandPosition = placedHand.transform.position + Vector3.up * scrollInput * handPlaceMoveSpeed;

                float distanceFromPlayer = Vector3.Distance(playerTransform.position, newHandPosition);

                if (distanceFromPlayer <= handPlaceRadius)
                {
                    placedHand.transform.position = newHandPosition;
                }
                else
                {
                    Vector3 direction = (newHandPosition - playerTransform.position).normalized;
                    placedHand.transform.position = playerTransform.position + direction * handPlaceRadius;
                }

                placedHand.transform.position = Vector3.MoveTowards(placedHand.transform.position,
                    newHandPosition, handPlaceMoveSpeed * Time.deltaTime);
            }

            UpdateHandRotation();
        }
    }

    public void FinalizePlacement()
    {
        Vector3 currentPosition = placedHand.transform.position;
        Quaternion handRotation = placedHand.transform.rotation;
        Destroy(placedHand);
        placedHand = Instantiate(GodsHand, currentPosition, handRotation);

        isSlomo = false;

        handIsActive = false;
        canCountLife = true;

        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = defFixedUpdateSpeed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CancelHandPlacement() {
        Destroy(placedHand);
        isSlomo = false;
        handIsActive = false;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = defFixedUpdateSpeed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void UpdateHandRotation()
    {
        Vector3 lookDirection = playerTransform.position - placedHand.transform.position;
        lookDirection.y = 0;
        placedHand.transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void RotatePlayer()
    {
        Vector3 flattenedMoveInput = new Vector3(playerMoveInput.x, 0, playerMoveInput.z);

        if (flattenedMoveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flattenedMoveInput, Vector3.up);

            rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, targetRotation, rotationSpeedMultiplier * Time.fixedDeltaTime);
        }
    }

    private void PlayerMiscAtributes()
    {
        if (coyoteTimeCounter < -1.5f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (coyoteTimeCounter < -0.5f)
        {
            x = true;
        }

        if (x && (playerIsGrounded || standingOnHand))
        {
            if (GroundedAfterLandingTimer > 0.0f)
            {
                wasAirborne = true;
                GroundedAfterLandingTimer -= Time.fixedDeltaTime;
            }
            else
            {
                x = false;
                wasAirborne = false;
            }
        }

        if (!wasAirborne)
        {
            GroundedAfterLandingTimer = 0.02f;
        }
        if (input.MoveIsPressed && (playerIsGrounded || standingOnHand) && !isDashing && !isBoosting) 
        { 
            if (input.RunIsPressed)
            {
                isRunning = true;
                isWalking = false;
            }
            else
            {
                isWalking = true;
            }
        }
        else 
        {
            isWalking = false;
            isRunning = false;
        }
    }

    private void SetBarValues()
    {
        if (BoostTimer <= 0.0f)
        {
            BoostSlider.value = BoostDelay;
        }
        else
        {
            BoostSlider.value = BoostDelay - BoostTimer;
        }

        if (DashTimer <= 0.0f)
        {
            DashSlider.value = DashDelay;
        }
        else
        {
            DashSlider.value = DashDelay - DashTimer;
        }

        if (!canPlaceHand) {
            
            if (canCountLife)
            {
                if (handLifeTime >= 0.0f && handLifeTime <= 5.0f)
                {
                    HandSlider.value = handLifeTime;
                }
                else
                {
                    HandSlider.value = 0.0f;
                }
            }

            if (canCountRecharge)
            {
                if (handPlaceRecharge >= 0.0f && handPlaceRecharge <= 10.0f)
                {
                    HandSlider.value = handPlaceRecharge / 2;
                }
                else
                {
                    HandSlider.value = 5.0f;
                }
            } 
        }
        else
        {
            HandSlider.value = 5.0f;
        }
        

        if (!canBoost)
        {
            BoostImg.gameObject.SetActive(true);
        }
        else { 
            BoostImg.gameObject.SetActive(false);
        }

        if (!canDash)
        {
            DashImg.gameObject.SetActive(true);
        }
        else
        {
            DashImg.gameObject.SetActive(false);
        }

        if (!canPlaceHand)
        {
            HandImg.gameObject.SetActive(true);
        }
        else
        {
            HandImg.gameObject.SetActive(false);
        }
    }
}
