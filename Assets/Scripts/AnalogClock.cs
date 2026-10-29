using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Drives an analog wall clock's hour, minute and second hands, plus a digital readout,
/// from the device's system clock. No animation clips are used: the hand rotations are
/// calculated every frame from DateTime.Now.
/// </summary>
public class AnalogClock : MonoBehaviour
{
    [Tooltip("Pivot of the hour hand, at the centre of the clock face")]
    public Transform hourHand = null;

    [Tooltip("Pivot of the minute hand, at the centre of the clock face")]
    public Transform minuteHand = null;

    [Tooltip("Pivot of the second hand, at the centre of the clock face")]
    public Transform secondHand = null;

    [Tooltip("Digital time readout (optional)")]
    public TextMeshProUGUI digitalTime = null;

    [Tooltip("Digital date readout (optional)")]
    public TextMeshProUGUI digitalDate = null;

    [Tooltip("Sweep the second hand smoothly instead of ticking once per second")]
    public bool smoothSeconds = false;

    private void Update()
    {
        DateTime now = DateTime.Now;

        float seconds = now.Second + (smoothSeconds ? now.Millisecond / 1000f : 0f);
        float minutes = now.Minute + seconds / 60f;
        float hours = now.Hour % 12 + minutes / 60f;

        // A full turn is 360 degrees: 360 / 12 = 30 degrees per hour, 360 / 60 = 6 degrees per minute or second.
        // The minute and hour hands include the fraction of the next unit, so they move continuously.
        SetHandAngle(hourHand, hours * 30f);
        SetHandAngle(minuteHand, minutes * 6f);
        SetHandAngle(secondHand, seconds * 6f);

        if (digitalTime != null)
            digitalTime.text = now.ToString("HH:mm:ss");

        if (digitalDate != null)
            digitalDate.text = now.ToString("dddd, d MMMM yyyy");
    }

    private static void SetHandAngle(Transform hand, float degreesClockwise)
    {
        // Facing the clock, a positive rotation around its Z axis turns counter-clockwise, so negate it.
        if (hand != null)
            hand.localRotation = Quaternion.Euler(0f, 0f, -degreesClockwise);
    }
}
