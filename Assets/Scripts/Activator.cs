using UnityEngine;
using UnityEngine.Events;

public class Activator : MonoBehaviour
{
    [Tooltip("Current activator state")]
    public bool isActivated;

    [Tooltip("Should this activator act as a toggle")]
    public bool isToggle;

    [Tooltip("Event fired when the activator is activated")]
    public UnityEvent onActivated;

    [Tooltip("Event fired when the activator is deactivated")]
    public UnityEvent onDeactivated;

    public void Activate()
    {
        onActivated.Invoke();
        isActivated = true;
    }

    public void Deactivate()
    {
        onDeactivated.Invoke();
        isActivated = false;
    }

    public void Toggle()
    {
        var newIsActivated = !isActivated;

        if (newIsActivated)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }

        // State is set last in-case events error.
        isActivated = newIsActivated;
    }
}
