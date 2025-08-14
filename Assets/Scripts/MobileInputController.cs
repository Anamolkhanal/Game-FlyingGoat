using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MobileInputController : MonoBehaviour
{
    public float minSwipeDistance = 80f; // pixels for swipe left/right
    public float tapMaxDistance = 30f;   // pixels to still count as tap
    public float tapMaxTime = 0.25f;     // seconds max for a tap

    private PlayerMovement playerMovement;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable(); // allow mouse-as-touch in Editor
    }

    void OnDisable()
    {
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (playerMovement == null) return;

        float externalDir = 0f;

        if (Touch.activeTouches.Count > 0)
        {
            var touch = Touch.activeTouches[0];
            var pos = touch.screenPosition;

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                var delta = pos - touch.startScreenPosition;
                if (Mathf.Abs(delta.x) >= minSwipeDistance && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    externalDir = Mathf.Sign(delta.x);
                }
            }

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                var totalDelta = pos - touch.startScreenPosition;
                var duration = (float)(UnityEngine.Time.time - touch.startTime);

                if (totalDelta.magnitude <= tapMaxDistance && duration <= tapMaxTime)
                {
                    playerMovement.TriggerExternalJump();
                }

                externalDir = 0f;
            }
        }

        playerMovement.SetExternalDirection(externalDir);
    }
}
