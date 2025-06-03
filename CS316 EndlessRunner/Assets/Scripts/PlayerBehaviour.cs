using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;
    [Tooltip("How fast the ball moves forward")]
    [Range(0,10)]
    public float rollSpeed = 5;

    private MobileJoystick joystick;

    public enum MobileHorizMovement
    {
        Accelerometer,
        ScreenTouch
    }
    [Tooltip("What Horizontal movemtn type should be used")]
    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;


    [Header("Swipe Properties")]
    [Tooltip("How far will the player move upon swiping")]
    public float swipeMove = 2f;
    [Tooltip("How far must the player swipe before we will execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;
    private float minSwipeDistancePixels;
    private Vector2 touchStart;

    [Header("Scale Propertis")]
    [Tooltip("The minimum size (In Unity Units) that the player should be")]
    public float minScale = 0.25f;
    [Tooltip("The maximum size (In Unity Units) that the player should be")]
    public float maxScale = 3.0f;
    private float currentScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;

        joystick = GameObject.FindObjectOfType<MobileJoystick>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PauseScreenBehaviour.paused)
        {
            return;
        }
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        if(joystick && joystick.axisValue.x != 0)
        {
            horizontalSpeed = joystick.axisValue.x * dodgeSpeed;
        }
        switch (horizMovement)
        {
            case MobileHorizMovement.Accelerometer:
                horizontalSpeed = Input.acceleration.x * dodgeSpeed;
                break;
            case MobileHorizMovement.ScreenTouch:
                if(Input.touchCount > 0)
                {
                    var firstTouch = Input.touches[0];
                    var screenPos = firstTouch.position;
                    horizontalSpeed = CalculateMovement(screenPos);
                }
                break;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            TouchObjectsPC(screenPos);
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            TouchObjectsMobile(touch);
            SwipeTeleport(touch);
            ScalePlayer();
        }
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            var pauseBehaviour = GameObject.FindObjectOfType<PauseScreenBehaviour>();
            pauseBehaviour.SetPauseMenu(!PauseScreenBehaviour.paused);
        }
        if (PauseScreenBehaviour.paused)
        {
            return;
        }
    }
    private float CalculateMovement(Vector3 screenPos)
    {
        var cam = Camera.main;

        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

         if (viewPos.x < 0.5f)
         {
            xMove = -1;
         }
         else
         {
            xMove = 1;
         }
         return xMove * dodgeSpeed;
    }
    private void SwipeTeleport(Touch touch)
    {
        if(touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnd = touch.position;
            float x = touchEnd.x - touchStart.x;
            if(Mathf.Abs(x) < minSwipeDistancePixels)
            {
                return;
            }
            Vector3 moveDirection;
            if(x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                moveDirection = Vector3.right;
            }
            RaycastHit hit;
            if(!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                var movement = moveDirection * swipeMove;
                var newPos = rb.position + movement;

                rb.MovePosition(newPos);
            }
        }
    }
    private void ScalePlayer()
    {
        if(Input.touchCount != 2)
        {
            return;
        }
        else
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 t0Pos = touch0.position;
            Vector2 t0Delta = touch0.deltaPosition;

            Vector2 t1Pos = touch1.position;
            Vector2 t1Delta = touch1.deltaPosition;

            Vector2 t0Prev = t0Pos - t0Delta;
            Vector2 t1Prev = t1Pos - t1Delta;

            float prevTDeltaMag = (t0Prev - t1Prev).magnitude;
            float tDelatMag = (t0Pos - t1Pos).magnitude;
            float deltaMagDiff = (prevTDeltaMag - tDelatMag);

            float newScale = currentScale;
            newScale -= (deltaMagDiff * Time.deltaTime);
            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            transform.localScale = Vector3.one * newScale;

            currentScale = newScale;
        }
    }
    private static void TouchObjectsPC(Vector2 screenPos)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        int layerMask = ~0;

        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
    void TouchObjectsMobile(Touch touch)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        int layerMask = ~0;

        if(Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }
}
