using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
	        public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 5f;     //30f - default
            public float jumpForceHangY = 10f;
            public float jumpForceHangX = 7f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}
#if !MOBILE_INPUT
	            if (Input.GetKey(RunKey))
	            {
		            CurrentTargetSpeed *= RunMultiplier;
		            m_Running = true;
	            }
	            else
	            {
		            m_Running = false;
	            }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }


        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }


        public Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();

        public GameObject gripPrefab;  // prefab of the grip object
        public GameObject jumpPrefab;  // prefab of the jump pad object
        public GameObject dashPrefab;  // prefab of the dash object
        public int ObjLimit = 0;    //Spawn object limiter (also to be used in Text UI script "ObjLimitScript.cs")

        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


        // variables holding data regarding the grip spawning and sphere cast for the grips
        private bool m_LeftDoesGripTraceExist, m_LeftCanStartGripTrace;

        //Variables holding data regarding the grip despawning and sphere cast for the grips
        private bool m_RightDoesGripTraceExist, m_RightCanStartGripTrace;

        private bool m_isHanging;  // is the player hanging from a grip


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
        }


        private void Update()
        {
            RotateView();

            //if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            if (Input.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }

        }


        private void FixedUpdate()
        {
            GroundCheck();
            Vector2 input = GetInput();

            // if input is detected on the horizontal or vertical axes and the player is on the ground or can move in the air then change the force acting on the rigidbody
            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                // the desiredMove vector direction the player wants to move in is the direction of a vector created by the camera direction
                // before applying magnitude to the desiredMove vector, the vector must be applied to the world by getting its location on a plane that represents the
                // ground / bottom of the world. this is normalised as to not effect the magnitude calculations. the magnitude of the desiredMove vector is equal to itself
                // multiplied by the speed of the players current movement type. the desiredMove force is only applied to the rigidbody if the velocity's speed/magnitude 
                // does not exceed the max speed.

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            // if the player is on the ground, a drag force is applied, if not, the drag force is removed
            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else if (m_isHanging)
            {
                Vector3 desiredJump = cam.transform.forward + cam.transform.up;
                desiredJump = Vector3.ProjectOnPlane(desiredJump, m_GroundContactNormal).normalized;

                desiredJump.x = desiredJump.x * movementSettings.jumpForceHangX;
                desiredJump.y = desiredJump.y * movementSettings.jumpForceHangY;
                desiredJump.z = desiredJump.z * movementSettings.jumpForceHangX;

                if ((m_RigidBody.velocity.sqrMagnitude < (movementSettings.jumpForceHangX * movementSettings.jumpForceHangY)) && m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.useGravity = true;
                    m_RigidBody.AddForce(desiredJump, ForceMode.Impulse);
                    m_Jumping = true;
                    m_isHanging = false;
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    StickToGroundHelper();
                }
            }
            m_Jump = false;

            // if the left mouse button is pressed down and there is no sphere trace for the grip,
            // allow sphere traces for the grip to start
            if (Input.GetKeyDown(KeyCode.Mouse0) && (m_LeftDoesGripTraceExist == false))
            {
                m_LeftCanStartGripTrace = true;
                m_LeftDoesGripTraceExist = true;
            }

            // if the left mouse button is released and a sphere trace for the grip can start,
            // start the grip trace
            if (Input.GetKeyUp(KeyCode.Mouse0) && m_LeftCanStartGripTrace)
            {
                m_LeftDoesGripTraceExist = true;
                m_LeftCanStartGripTrace = false;

                SpawnGrip();
            }

            // if the left mouse button is pressed down and there is no sphere trace for the grip,
            // allow sphere traces for the grip to start
            if (Input.GetKeyDown(KeyCode.Mouse1) && (m_RightDoesGripTraceExist == false))
            {
                m_RightCanStartGripTrace = true;
                m_RightDoesGripTraceExist = true;
            }

            // if the left mouse button is released and a sphere trace for the grip can start,
            // start the grip trace
            if (Input.GetKeyUp(KeyCode.Mouse1) && m_RightCanStartGripTrace)
            {
                m_RightDoesGripTraceExist = true;
                m_RightCanStartGripTrace = false;

                DespawnGrip();
            }
        }


        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }


        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                //x = CrossPlatformInputManager.GetAxis("Horizontal"),
                //y = CrossPlatformInputManager.GetAxis("Vertical")
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation (transform, cam.transform);

            if (m_IsGrounded || advancedSettings.airControl)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
            }
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && (m_IsGrounded || m_isHanging) && m_Jumping)
            {
                m_Jumping = false;
            }
        }

        private void SpawnGrip()
        {
            if (ObjLimit < 5)
            {
                float gripRadius = (gripPrefab.GetComponentInChildren<BoxCollider>().size.x / 4); // original 2 //gripPrefab.GetComponent<BoxCollider>().size.x / 2);
                RaycastHit hitInfoGrip;
                if (Physics.SphereCast(cam.transform.position, gripRadius, cam.transform.forward, out hitInfoGrip, 1000))
                {
                    ObjLimit++;
                    if (hitInfoGrip.collider.tag == "Wall")
                    {
                        GameObject gripObject = Instantiate(gripPrefab);
                        //gripObject.transform.GetChild(0).transform.localPosition = (cam.transform.forward * -0.5f);
                        gripObject.transform.GetChild(0).transform.localPosition = (hitInfoGrip.collider.transform.forward * 0.5f);
                        gripObject.transform.GetChild(0).transform.forward = hitInfoGrip.collider.transform.forward;
                        gripObject.transform.position = hitInfoGrip.point;
                        gripObject.GetComponentInChildren<BoxCollider>().isTrigger = true;

                        //Debug.Log(cam.transform.forward);
                    }
                    else if (hitInfoGrip.collider.tag == "Grip")
                    {
                        GameObject jumpPadObject = Instantiate(jumpPrefab);
                        jumpPadObject.transform.forward = hitInfoGrip.collider.transform.forward;
                        jumpPadObject.transform.position = hitInfoGrip.collider.transform.position;
                        jumpPadObject.transform.GetChild(0).transform.localPosition = (hitInfoGrip.collider.transform.forward * 0.5f);

                        Destroy(hitInfoGrip.collider.transform.parent.gameObject); //hitInfoGrip.collider.gameObject);

                        jumpPadObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
                    }
                    else if (hitInfoGrip.collider.tag == "JumpPad")
                    {
                        GameObject dashObject = Instantiate(dashPrefab);
                        dashObject.transform.forward = hitInfoGrip.collider.transform.forward;
                        dashObject.transform.position = hitInfoGrip.collider.transform.position;
                        dashObject.transform.GetChild(0).transform.localPosition = (hitInfoGrip.collider.transform.forward * 0.5f);

                        Destroy(hitInfoGrip.collider.transform.parent.gameObject);

                        dashObject.GetComponentInChildren<BoxCollider>().isTrigger = true;

                    }
                }
            }

            m_LeftDoesGripTraceExist = false;
            m_LeftCanStartGripTrace = true;
        }

        private void DespawnGrip()
        {
            if (ObjLimit > 0)
            {
                float gripRadius = (gripPrefab.GetComponentInChildren<BoxCollider>().size.x / 2); // original 2 //gripPrefab.GetComponent<BoxCollider>().size.x / 2);
                RaycastHit hitInfoGrip;
                if (Physics.SphereCast(cam.transform.position, gripRadius, cam.transform.forward, out hitInfoGrip, 1000))
                {
                    ObjLimit--;
                    if (hitInfoGrip.collider.tag == "Grip")
                    {
                        Destroy(hitInfoGrip.collider.transform.parent.gameObject);
                    }
                    else if (hitInfoGrip.collider.tag == "JumpPad")
                    {
                        GameObject gripObject = Instantiate(gripPrefab);
                        gripObject.transform.GetChild(0).transform.localPosition = (hitInfoGrip.collider.transform.forward * 0.5f);
                        gripObject.transform.GetChild(0).transform.forward = hitInfoGrip.collider.transform.forward;
                        gripObject.transform.position = hitInfoGrip.point;

                        gripObject.GetComponentInChildren<BoxCollider>().isTrigger = true;

                        Destroy(hitInfoGrip.collider.transform.parent.gameObject);
                    }
                    else if(hitInfoGrip.collider.tag == "Dash")
                    {
                        GameObject jumpPadObject = Instantiate(jumpPrefab);
                        jumpPadObject.transform.forward = hitInfoGrip.collider.transform.forward;
                        jumpPadObject.transform.position = hitInfoGrip.collider.transform.position;
                        jumpPadObject.transform.GetChild(0).transform.localPosition = (hitInfoGrip.collider.transform.forward * 0.5f);

                        jumpPadObject.GetComponentInChildren<BoxCollider>().isTrigger = true;

                        Destroy(hitInfoGrip.collider.transform.parent.gameObject);
                    }
                }
            }

            m_RightDoesGripTraceExist = false;
            m_RightCanStartGripTrace = true;
        }


        public void setHang(bool isHang)
        {
            m_isHanging = isHang;

            // if hanging, set location to point of collision
            // above is done in grip script
        }
    }
}
