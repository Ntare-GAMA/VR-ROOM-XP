using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Ray Interactor version of ToggleNearFarAttach: switches whether grabbed objects
/// stay at the end of the ray (distance grab) or snap to the hand (Force Grab).
/// </summary>
public class ToggleRayDistanceGrab : MonoBehaviour
{
    [Tooltip("The ray interactors to switch")]
    public XRRayInteractor[] rayInteractors = new XRRayInteractor[0];

    /// <summary>
    /// Call this from a UI Toggle's OnValueChanged event.
    /// </summary>
    /// <param name="useDistanceGrab">
    /// true = grabbed objects stay at the ray hit point,
    /// false = grabbed objects snap to the hand.
    /// </param>
    public void SetFarAttachMode(bool useDistanceGrab)
    {
        foreach (var ray in rayInteractors)
        {
            if (ray != null)
                ray.useForceGrab = !useDistanceGrab;
        }
    }
}
