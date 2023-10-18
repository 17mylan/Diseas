using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using KinematicCharacterController;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using TMPro;

namespace KinematicCharacterController.Examples
{
    public enum CharacterState
    {
        Default,
    }

    public enum OrientationMethod
    {
        TowardsCamera,
        TowardsMovement,
    }

    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;
    }

    public struct AICharacterInputs
    {
        public Vector3 MoveVector;
        public Vector3 LookVector;
    }

    public enum BonusOrientationMethod
    {
        None,
        TowardsGravity,
        TowardsGroundSlopeAndGravity,
    }

    public class ExampleCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 10f;
        public float velocityMagnitude;
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsCamera;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 15f;
        public float AirAccelerationSpeed = 15f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public float JumpUpSpeed = 10f;
        public float DoubleJumpUpSpeed = 2f;
        public float JumpScalableForwardSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0f;
        public float JumpPostGroundingGraceTime = 0f;
        public bool canDoubleJump = false;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;

        [Header("Dashing")]
        public GameObject _playerReference;
        public float _dashCooldown = 5f;
        public bool _isDashing = false;
        public bool _canDash = true;
        public bool _canDashBecausePlayerIsMoving = false;
        public float _dashSpeed = 20f;
        public float _dashDuration = 0.2f;
        public float _dashTimer = 0f;

        [Header("Bullet")]
        public Transform spawnPoint;
        public GameObject bulletPrefab;
        public float bulletSpeed = 100f;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new List<Collider>();
        public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
        public float BonusOrientationSharpness = 10f;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Transform MeshRoot;
        public Transform CameraFollowPoint;
        public float CrouchedCapsuleHeight = 1f;
        public Animator playerAnimator;
        public CharacterState CurrentCharacterState { get; private set; }

        private Collider[] _probedColliders = new Collider[8];
        private RaycastHit[] _probedHits = new RaycastHit[8];
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _shouldBeCrouching = false;
        private bool _isCrouching = false;
        public bool _isAiming = false;
        public Texture2D cursorTexture;
        private Vector3 lastInnerNormal = Vector3.zero;
        private Vector3 lastOuterNormal = Vector3.zero;
        public Enemy _enemy;
        public Tuto tuto;
        public Teleportation teleportation;
        public CompanionAI companionAI;
        public ExampleCharacterCamera exampleCharacterCamera;
        private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;
        public PauseMenu pauseMenu;
        [Header("Tuto Reference")]
        public GameObject wallDashReference;
        [Header("Platforming Capacity")]
        public bool _hasPlatformingCapacity = false;
        public TimerPlatforms _timerPlatforms;
        public Material _dontHaveCapacity;
        public Material _hasCapacity;
        public List<GameObject> CapacityPlatforms = new List<GameObject>();
        [Header("Superpuissance Capacity")]
        public bool _hasSuperpuissanceCapacity = false;
        public TimerSuperpuissance _timerSuperpuissance;
        [Header("Double Jump Capacity")]
        public bool _hasDoubleJumpCapacity = false;
        public TimerDoubleJump _timerDoubleJump;
        [Header("Collectible")]
        public int currentCollectibleNumber;
        public TextMeshProUGUI currentCollectibleNumerText;
        [Header("Enemy Head Detector")]
        public float raycastDistance = 3f;

        private void Awake()
        {
            // Handle initial state
            TransitionToState(CharacterState.Default);

            // Assign the characterController to the motor
            Motor.CharacterController = this;
        }

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState)
        {
            CharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState)
        {
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Move and look inputs
                        _moveInputVector = cameraPlanarRotation * moveInputVector;

                        switch (OrientationMethod)
                        {
                            case OrientationMethod.TowardsCamera:
                                _lookInputVector = cameraPlanarDirection;
                                break;
                            case OrientationMethod.TowardsMovement:
                                _lookInputVector = _moveInputVector.normalized;
                                break;
                        }

                        // Jumping input
                        if (inputs.JumpDown)
                        {
                            //Detecter le double jump
                            if(_jumpConsumed && canDoubleJump)
                            {
                                _jumpConsumed = false;
                                Vector3 additionalVelocity = new Vector3(0f, JumpUpSpeed * DoubleJumpUpSpeed, 0f);
                                AddVelocity(additionalVelocity);
                                playerAnimator.SetFloat("Jump", 0f);
                                playerAnimator.SetFloat("Jump", 1f);
                            }
                            else if(!_jumpedThisFrame)
                            {
                                _timeSinceJumpRequested = 0f;
                                _jumpRequested = true;
                            }
                        }

                        // Crouching input
                        /*if (inputs.CrouchDown)
                        {
                            _shouldBeCrouching = true;
                            if (!_isCrouching)
                            {
                                _isCrouching = true;
                                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                                MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);

                            }
                        }
                        else if (inputs.CrouchUp)
                        {
                            _shouldBeCrouching = false;
                        }*/
                        if (Input.GetKeyDown(KeyCode.R) && _canDash && _canDashBecausePlayerIsMoving && !_jumpRequested && !_jumpConsumed) 
                        {
                            _isDashing = true;
                            _dashTimer = 0f;
                            //print("Je dash");
                            playerAnimator.SetFloat("Dash", 1f);
                            StartCoroutine(DashCooldown());
                            StartCoroutine(DashCameraZoom());
                            if(SceneManager.GetActiveScene().name == "TutorialRoom")
                            {
                                if(tuto.isTutoEnabled && tuto.hasPassedColliderDetectorToDestroyWallForDash)
                                    tuto.DestroyWallWhenPlayerDashedOnTutoEnabled();
                            }
                        }
                        if(Input.GetMouseButtonDown(1) && companionAI.isCompanionFree)
                        {
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            _isAiming = true;
                            //Centrer le cursor
                            Vector2 cursorOffset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
                            Cursor.SetCursor(cursorTexture,cursorOffset, CursorMode.Auto);
                        }
                        else if(Input.GetMouseButtonUp(1))
                        {
                            Cursor.lockState = CursorLockMode.Locked;
                            Cursor.visible = false;
                            _isAiming = false;
                            Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs)
        {
            _moveInputVector = inputs.MoveVector;
            _lookInputVector = inputs.LookVector;
        }

        private Quaternion _tmpTransientRot;

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                        }

                        Vector3 currentUp = (currentRotation * Vector3.up);
                        if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
                        {
                            if (Motor.GroundingStatus.IsStableOnGround)
                            {
                                Vector3 initialCharacterBottomHemiCenter = Motor.TransientPosition + (currentUp * Motor.Capsule.radius);

                                Vector3 smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                                // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                                Motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * Motor.Capsule.radius));
                            }
                            else
                            {
                                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                            }
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Ground movement
                        if (Motor.GroundingStatus.IsStableOnGround)
                        {
                            float currentVelocityMagnitude = currentVelocity.magnitude;

                            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                            // Reorient velocity on slope
                            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                            // Calculate target velocity
                            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                            Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                            // Smooth movement Velocity
                            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));


                            if (currentVelocity.magnitude >= 1f)
                            {
                                _canDashBecausePlayerIsMoving = true;
                                playerAnimator.SetFloat("Speed", 1f);
                                //print("Je marche");
                            }
                            else if(currentVelocity.magnitude < 1f)
                            {
                                _canDashBecausePlayerIsMoving = false;
                                playerAnimator.SetFloat("Speed", 0f);
                                //print("Je suis a l'arrêt"); 
                            }
                            if (_isDashing)
                            {
                                _dashTimer += deltaTime;
                                Vector3 dashVelocity = _moveInputVector.normalized * _dashSpeed;
                                currentVelocity = dashVelocity;
                                if (_dashTimer >= _dashDuration)
                                {
                                    _isDashing = false;
                                    //print("Je Dash plus");
                                    playerAnimator.SetFloat("Dash", 0f);
                                }
                            }
                        }
                        // Air movement
                        else
                        {
                            // Add move input
                            if (_moveInputVector.sqrMagnitude > 0f)
                            {
                                Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;

                                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                                // Limit air velocity from inputs
                                if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                                {
                                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                }
                                else
                                {
                                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    {
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                    }
                                }

                                // Prevent air-climbing sloped walls
                                if (Motor.GroundingStatus.FoundAnyGround)
                                {
                                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                    {
                                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                    }
                                }
                                // Apply added velocity
                                currentVelocity += addedVelocity;
                            }

                            // Gravity
                            currentVelocity += Gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (Drag * deltaTime)));
                        }

                        // Handle jumping
                        _jumpedThisFrame = false;
                        _timeSinceJumpRequested += deltaTime;
                        if (_jumpRequested && !_isDashing && velocityMagnitude < 10f)
                        {
                            // See if we actually are allowed to jump
                            if (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                            {
                                // Calculate jump direction before ungrounding
                                Vector3 jumpDirection = Motor.CharacterUp;
                                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                                {
                                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                                }

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                Motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;
                                playerAnimator.SetFloat("Jump", 1f);
                                //print("Je saute");
                            }
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }
                        velocityMagnitude = currentVelocity.magnitude;
                        break;
                    }
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }

                        // Handle uncrouching
                        if (_isCrouching && !_shouldBeCrouching)
                        {
                            // Do an overlap test with the character's standing height to see if there are any obstructions
                            Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                            if (Motor.CharacterOverlap(
                                Motor.TransientPosition,
                                Motor.TransientRotation,
                                _probedColliders,
                                Motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0)
                            {
                                // If obstructions, just stick to crouching dimensions
                                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                            }
                            else
                            {
                                // If no obstructions, uncrouch
                                MeshRoot.localScale = new Vector3(1f, 1f, 1f);
                                _isCrouching = false;
                            }
                        }
                        break;
                    }
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders.Count == 0)
            {
                return true;
            }

            if (IgnoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            if(SceneManager.GetActiveScene().name == "TutorialRoom")
            {
                if(hitCollider.gameObject == tuto.teleporterToPart2)
                {
                    teleportation.StartCoroutine(teleportation.TeleportPlayerWithDelay("TeleportationFromPart1toPart2", 1f));
                }
                else if(hitCollider.gameObject == tuto.teleporterToPart3)
                {
                    teleportation.StartCoroutine(teleportation.TeleportPlayerWithDelay("TeleportationFromPart2toPart3", 1f));
                }
                else if(hitCollider.gameObject == tuto.teleportToGymRoom)
                {
                    teleportation.StartCoroutine(teleportation.TeleportPlayerWithDelay("TeleportationFromPart3toGymRoom", 1f));
                }
            }
        }
        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            RaycastHit hitInfo;
    
            if (Physics.Raycast(transform.position, hitPoint - transform.position, out hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                
                if (hitObject.CompareTag("Enemy") && _isDashing && hitObject.GetComponent<Enemy>()._isStun)
                {
                    // Determiner quel enemi est touché pour donner le pouvoir spécial
                    // Ne pas oublier dans le script "Enemy" de faire la méthode pour donner les pouvoirs

                    Destroy(hitCollider.gameObject);

                    if (hitCollider.gameObject.GetComponent<EnemyPlatforms>() != null)
                    {
                        GiveCapacityToPlayer("Platforming");
                    }
                    else if (hitCollider.gameObject.GetComponent<EnemySuperpuissance>() != null && _hasSuperpuissanceCapacity)
                    {
                        GiveCapacityToPlayer("Superpuissance");
                    }
                    else if (hitCollider.gameObject.GetComponent<EnemyDoubleJump>() != null)
                    {
                        GiveCapacityToPlayer("DoubleJump");
                    }

                }
                else if(hitObject.CompareTag("EnemyBack") && _isDashing && hitObject.transform.parent.gameObject.GetComponent<Enemy>()._isStun)
                {
                    Destroy(hitCollider.transform.parent.gameObject);
                    GiveCapacityToPlayer("Superpuissance");
                }
                else if(SceneManager.GetActiveScene().name == "TutorialRoom")
                {
                    if(hitCollider == tuto.colliderDetectorToDash)
                    {
                        Destroy(tuto.colliderDetectorToDash);
                        tuto.hasPassedColliderDetectorToDestroyWallForDash = true;
                    }
                }
            }
        }
        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            if (hitCollider.gameObject.tag == "Collectible")
            {
                currentCollectibleNumber += 1;
                currentCollectibleNumerText.text = currentCollectibleNumber.ToString();
                Destroy(hitCollider.gameObject);
            }
        }

        protected void OnLanded()
        {
            playerAnimator.SetFloat("Jump", 0f);
            if(_isDashing)
            {
                _isDashing = false;
                playerAnimator.SetFloat("Dash", 0f);
            }
        }

        protected void OnLeaveStableGround()
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
        public void Start()
        {
            _enemy = FindObjectOfType<Enemy>();
            _timerPlatforms = FindObjectOfType<TimerPlatforms>();
            _timerSuperpuissance = FindObjectOfType<TimerSuperpuissance>();
            _timerDoubleJump = FindObjectOfType<TimerDoubleJump>();
            tuto = FindObjectOfType<Tuto>();
            teleportation = FindObjectOfType<Teleportation>();
            companionAI = FindObjectOfType<CompanionAI>();
            exampleCharacterCamera = FindObjectOfType<ExampleCharacterCamera>();
            pauseMenu = FindObjectOfType<PauseMenu>();

            currentCollectibleNumber = 0;
            currentCollectibleNumerText.text = currentCollectibleNumber.ToString();
        }
        public void Update()
        {
            HeadDetector();
            /*if(Input.GetMouseButton(1) && companionAI.isCompanionFree)
            {
                exampleCharacterCamera.SetFollowTransform(companionAI.companionTransformCamera);
            }
            if(Input.GetMouseButtonUp(1) && companionAI.isCompanionFree)
            {
                exampleCharacterCamera.SetFollowTransform(CameraFollowPoint);
            }*/
            if (Input.GetMouseButtonDown(0) && companionAI.isCompanionFree && !pauseMenu.gameIsPaused)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if(_isAiming)
                    {
                        Vector3 targetPoint = hit.point;
                        targetPoint += new Vector3(0, 0, 0);

                        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
                        Vector3 direction = (targetPoint - spawnPoint.position).normalized;
                        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
                    }
                }
            }
        }
        public IEnumerator DashCooldown()
        {
            _canDash = false;
            yield return new WaitForSeconds(_dashCooldown);
            _canDash = true;
        }
        public IEnumerator DashCameraZoom()
        {
            exampleCharacterCamera.MaxDistance = exampleCharacterCamera.DashMinDistance;
            exampleCharacterCamera.DefaultDistance = exampleCharacterCamera.MaxDistance;
            exampleCharacterCamera.MinDistance = exampleCharacterCamera.DefaultDistance;
            yield return new WaitForSeconds(_dashDuration);
            exampleCharacterCamera.MaxDistance = exampleCharacterCamera.DashMaxDistance;
            exampleCharacterCamera.DefaultDistance = exampleCharacterCamera.MaxDistance;
            exampleCharacterCamera.MinDistance = exampleCharacterCamera.DefaultDistance;
        }
        public void SetPlatformingCapacityState(bool _State)
        {
            foreach (GameObject obj in CapacityPlatforms)
            {
                BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
                if(_State)
                {
                    obj.GetComponent<Renderer>().material = _hasCapacity;
                    boxCollider.isTrigger = false;
                    _hasPlatformingCapacity = true;
                }
                else
                {
                    obj.GetComponent<Renderer>().material = _dontHaveCapacity;
                    boxCollider.isTrigger = true;
                    _hasPlatformingCapacity = false;
                }
            }
        }
        public void GiveCapacityToPlayer(string _string)
        {
            if(_string == "Platforming")
            {
                if(!_hasPlatformingCapacity)
                {
                    SetPlatformingCapacityState(true);
                    if(!_timerPlatforms.isTimerStarted)
                        _timerPlatforms.StartTimer();
                }
                else if(_hasPlatformingCapacity)
                    _timerPlatforms.AddToTimer(30f);
            }
            else if(_string == "Superpuissance")
            {
                if(!_hasSuperpuissanceCapacity)
                {
                    if(!_timerSuperpuissance.isTimerStarted)
                    {
                        _timerSuperpuissance.StartTimer();
                        _hasSuperpuissanceCapacity = true;
                    }
                }
                else if(_hasSuperpuissanceCapacity)
                    _timerSuperpuissance.AddToTimer(5f);
            }
            else if(_string == "DoubleJump")
            {
                if(!_hasDoubleJumpCapacity)
                {
                    if (!_timerDoubleJump.isTimerStarted)
                    {
                        _timerDoubleJump.StartTimer();
                        _hasDoubleJumpCapacity = true;
                        canDoubleJump = true;
                    }
                }
                else if (_hasDoubleJumpCapacity)
                    _timerDoubleJump.AddToTimer(10f);
            }
        }
        public void HeadDetector()
        {
            Vector3 playerPosition = transform.position;
            Vector3 raycastDirection = Vector3.down;
            RaycastHit hit;
            if (Physics.Raycast(playerPosition, raycastDirection, out hit, raycastDistance))
            {
                Debug.DrawLine(playerPosition, hit.point, Color.red);
                if (hit.collider.tag == "EnemyHead")
                {
                    Destroy(hit.collider.transform.parent.gameObject);
                    if (SceneManager.GetActiveScene().name == "TutorialRoom" && tuto.isTutoEnabled && tuto.isInPhaseToJumpInHeadOfEnemies)
                        tuto.TutoAddEnemyKilledToSaveHisCompanion(1);
                }
            }
        }
    }
}