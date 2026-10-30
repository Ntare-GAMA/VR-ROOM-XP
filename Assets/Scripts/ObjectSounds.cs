using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Gives a grabbable object its sounds: a soft sound when a hand picks it up, and an impact sound
/// when it hits something (dropped, thrown, knocked over). The harder the hit, the louder the impact.
/// Uses a 3D Audio Source, so you hear where the object is.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ObjectSounds : MonoBehaviour
{
    [Tooltip("3D audio source that plays this object's sounds")]
    public AudioSource source = null;

    [Tooltip("Played when a hand or ray grabs the object")]
    public AudioClip grabClip = null;

    [Range(0f, 1f)]
    public float grabVolume = 0.4f;

    [Tooltip("One is picked at random for each impact, so repeated hits don't sound identical")]
    public AudioClip[] impactClips = new AudioClip[0];

    [Range(0f, 1f)]
    public float impactVolume = 1f;

    [Tooltip("Impacts slower than this (m/s) are silent, so resting objects don't keep clicking")]
    public float minImpactSpeed = 0.35f;

    [Tooltip("Impacts at or above this speed (m/s) play at full volume")]
    public float maxImpactSpeed = 4f;

    private const float Cooldown = 0.08f;
    private const float StartupSilence = 1f;

    private XRGrabInteractable grab;
    private float lastImpactTime = -1f;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
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

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Snapping into a socket is not a grab by the player, so it stays quiet.
        if (args.interactorObject is XRSocketInteractor)
            return;
        if (source != null && grabClip != null)
            source.PlayOneShot(grabClip, grabVolume);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Skip the first moment of the scene, while objects settle onto shelves and floors.
        if (Time.timeSinceLevelLoad < StartupSilence || Time.time - lastImpactTime < Cooldown)
            return;
        if (source == null || impactClips.Length == 0)
            return;

        float speed = collision.relativeVelocity.magnitude;
        if (speed < minImpactSpeed)
            return;

        lastImpactTime = Time.time;
        float strength = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, speed);
        AudioClip clip = impactClips[Random.Range(0, impactClips.Length)];
        source.pitch = Random.Range(0.92f, 1.08f);
        source.PlayOneShot(clip, impactVolume * Mathf.Lerp(0.15f, 1f, strength));
    }
}
