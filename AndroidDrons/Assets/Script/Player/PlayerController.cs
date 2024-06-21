using UnityEngine;
using UnityEngine.InputSystem;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController")]
        [SerializeField] public GameObject panelLook;
        [SerializeField] public FixedJoystick joystick;
        [SerializeField] public FixedJoystick joystickLook;
        [SerializeField] public VariableJoystick joystickHeight;
        [SerializeField] public Transform Camera;
        [SerializeField, Range(1, 100)] float walkingSpeed = 3.0f;
        [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
        [SerializeField, Range(2, 200)] float RuningSpeed = 4.0f;
        [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;

        [SerializeField, Range(0, 20)] float upSpeed = 6.0f;
        [SerializeField, Range(0, 20)] float downSpeed = 6.0f;

        [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
        [Space(20)]
        [Header("Advance")]
        [SerializeField] float CroughHeight = 1.0f;
        [SerializeField] float gravity = 20.0f;
        [SerializeField] float timeToRunning = 2.0f;
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool CanRunning = true;
        
        [Space(20)]
        [Header("Input")]
        [SerializeField] KeyCode CroughKey = KeyCode.LeftControl;


        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        bool isCrough = false;
        float InstallCroughHeight;
        float rotationX = 0;
        [HideInInspector] public bool isRunning = false;
        Vector3 InstallCameraMovement;
        Camera cam;
        [HideInInspector] public bool Moving;
        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;
        float RunningValue;
        float installGravity;
        bool WallDistance;
        [SerializeField, Range(1, 100)] public float WalkingSpeeds = 60 ;
        [HideInInspector] public float WalkingValue;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            //lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            InstallCroughHeight = characterController.height;
            InstallCameraMovement = Camera.localPosition;
            RunningValue = RuningSpeed;
            installGravity = gravity;
            WalkingValue = walkingSpeed;
            downSpeed = -downSpeed;
        }

        [System.Obsolete]
        void Update()
        {
            RaycastHit CroughCheck;

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            isRunning = !isCrough ? CanRunning ? Input.GetKey(KeyCode.LeftShift) : false : false;
            vertical = canMove ? (isRunning ? RunningValue : WalkingValue) * joystick.Vertical : 0;
            horizontal = canMove ? (isRunning ? RunningValue : WalkingValue) * joystick.Horizontal : 0;

            if (vertical < 0) { CanRunning = false; }
            else { CanRunning = true; }

            if (isRunning) RunningValue = Mathf.Lerp(RunningValue, RuningSpeed, timeToRunning * Time.deltaTime);
            else RunningValue = WalkingValue;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * vertical) + (right * horizontal);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            if (joystickHeight.Vertical > 0)
            {
                moveDirection.y = upSpeed;
            }
            if (joystickHeight.Vertical < 0)
            {
                moveDirection.y = downSpeed;
            }
            if (joystickHeight.Vertical == 0)
            {
                moveDirection.y = 0;
            }
            
            
            characterController.Move(moveDirection * Time.deltaTime);
            Moving = horizontal < 0 || vertical < 0 || horizontal > 0 || vertical > 0 ? true : false;

            float MoseX = joystickLook.Horizontal;
            float MoseY = joystickLook.Vertical;
            //float MoseX = 0;
            //float MoseY = 0;
            //try
            //{
            //    if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress && panelLook.active == true)
            //    {

            //        //if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
            //        //    return;
            //        MoseX = Touchscreen.current.touches[0].delta.ReadValue().x;
            //        MoseY = Touchscreen.current.touches[0].delta.ReadValue().y;
            //    }
            //}
            //catch { }
            MoseX *= lookSpeed;
            MoseY *= -lookSpeed;

            rotationX += MoseY * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, MoseX * lookSpeed, 0);

            


            if (Input.GetKey(CroughKey))
            {
                isCrough = true;
                float Height = Mathf.Lerp(characterController.height, CroughHeight, 5 * Time.deltaTime);
                characterController.height = Height;
                WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);

            }
            else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
            {
                if (characterController.height != InstallCroughHeight)
                {
                    isCrough = false;
                    float Height = Mathf.Lerp(characterController.height, InstallCroughHeight, 6 * Time.deltaTime);
                    characterController.height = Height;
                    WalkingValue = Mathf.Lerp(WalkingValue, walkingSpeed, 4 * Time.deltaTime);
                }
            }


        }

        
       

    }
}
