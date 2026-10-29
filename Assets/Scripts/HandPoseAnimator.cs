using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Curls the fingers of a rigged hand from the controller's grip and trigger values, so the hand
/// mirrors what the player's real fingers are doing. Grip closes the middle, ring and little fingers,
/// the trigger closes the index finger, and the thumb follows whichever is pressed harder.
/// No animation clips are used: each joint is rotated from its rest pose every frame.
/// </summary>
public class HandPoseAnimator : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Analog grip value (0 to 1), e.g. XRI Left Interaction/Select Value")]
    public InputActionProperty gripAction;

    [Tooltip("Analog trigger value (0 to 1), e.g. XRI Left Interaction/Activate Value")]
    public InputActionProperty triggerAction;

    [Header("Finger joints, knuckle to tip")]
    public Transform[] thumb = new Transform[0];
    public Transform[] index = new Transform[0];
    public Transform[] middle = new Transform[0];
    public Transform[] ring = new Transform[0];
    public Transform[] pinky = new Transform[0];

    [Header("Pose")]
    [Tooltip("Local axis each joint bends around. For this rig, +X curls the fingers toward the palm")]
    public Vector3 curlAxis = Vector3.right;

    [Tooltip("How far each finger joint bends in a full fist, knuckle to tip (degrees)")]
    public Vector3 fingerCurl = new Vector3(70f, 95f, 60f);

    [Tooltip("How far each thumb joint bends in a full fist, base to tip (degrees)")]
    public Vector3 thumbCurl = new Vector3(10f, 30f, 45f);

    [Tooltip("How quickly the fingers follow the buttons")]
    public float responsiveness = 20f;

    private Quaternion[][] restRotations;
    private float grip;
    private float trigger;

    private void Awake()
    {
        Transform[][] fingers = { thumb, index, middle, ring, pinky };
        restRotations = new Quaternion[fingers.Length][];
        for (int f = 0; f < fingers.Length; f++)
        {
            restRotations[f] = new Quaternion[fingers[f].Length];
            for (int j = 0; j < fingers[f].Length; j++)
                restRotations[f][j] = fingers[f][j].localRotation;
        }
    }

    private void LateUpdate()
    {
        // Ease toward the button values so the fingers move smoothly instead of snapping.
        float blend = 1f - Mathf.Exp(-responsiveness * Time.deltaTime);
        grip = Mathf.Lerp(grip, ReadValue(gripAction), blend);
        trigger = Mathf.Lerp(trigger, ReadValue(triggerAction), blend);

        Curl(0, thumb, thumbCurl, Mathf.Max(grip, trigger * 0.6f));
        Curl(1, index, fingerCurl, trigger);
        Curl(2, middle, fingerCurl, grip);
        Curl(3, ring, fingerCurl, grip);
        Curl(4, pinky, fingerCurl, grip);
    }

    private void Curl(int finger, Transform[] joints, Vector3 fullCurl, float amount)
    {
        for (int j = 0; j < joints.Length && j < 3; j++)
            joints[j].localRotation = restRotations[finger][j] * Quaternion.AngleAxis(fullCurl[j] * amount, curlAxis);
    }

    private static float ReadValue(InputActionProperty property)
    {
        InputAction action = property.action;
        return action != null && action.enabled ? Mathf.Clamp01(action.ReadValue<float>()) : 0f;
    }
}
