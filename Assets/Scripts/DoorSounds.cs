using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Sounds for a hinged door: a creak that follows how fast the door swings (silent when still,
/// louder and slightly higher the faster it moves), a latch click when the handle is grabbed,
/// and a thud when the door bumps into something.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DoorSounds : MonoBehaviour
{
    [Tooltip("Looping 3D audio source playing the creak clip")]
    public AudioSource creakSource = null;

    [Tooltip("3D audio source for the latch click and bumps")]
    public AudioSource oneShotSource = null;

    [Tooltip("Played when the handle is grabbed")]
    public AudioClip latchClip = null;

    [Tooltip("Played when the door hits a wall or frame")]
    public AudioClip bumpClip = null;

    [Tooltip("Swing speed (degrees per second) where the creak starts")]
    public float minSwingSpeed = 6f;

    [Tooltip("Swing speed (degrees per second) where the creak is at full volume")]
    public float maxSwingSpeed = 90f;

    [Range(0f, 1f)]
    public float maxCreakVolume = 0.7f;

    private const float StartupSilence = 1f;

    private Rigidbody body;
    private XRGrabInteractable grab;
    private float creakVolume;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        if (creakSource != null)
        {
            creakSource.loop = true;
            creakSource.volume = 0f;
            if (!creakSource.isPlaying)
                creakSource.Play();
        }
    }

    private void OnEnable()
    {
        if (grab != null)
            grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        if (grab != null)
            grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void Update()
    {
        if (creakSource == null)
            return;

        float speed = body.angularVelocity.magnitude * Mathf.Rad2Deg;
        float target = Mathf.InverseLerp(minSwingSpeed, maxSwingSpeed, speed);

        // Ease the volume so the creak swells and fades instead of cutting in and out.
        creakVolume = Mathf.MoveTowards(creakVolume, target, Time.deltaTime * 4f);
        creakSource.volume = creakVolume * maxCreakVolume;
        creakSource.pitch = Mathf.Lerp(0.85f, 1.15f, target);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (oneShotSource != null && latchClip != null)
            oneShotSource.PlayOneShot(latchClip, 0.6f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.timeSinceLevelLoad < StartupSilence || oneShotSource == null || bumpClip == null)
            return;
        float speed = collision.relativeVelocity.magnitude;
        if (speed > 0.3f)
            oneShotSource.PlayOneShot(bumpClip, Mathf.Clamp01(speed / 2f));
    }
}
