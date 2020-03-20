using UnityEngine;
using UnityEngine.Events;

public class GrabbableToggleSwitch : MonoBehaviour
{
    [Tooltip("The grabbable toggle switch")]
    public Grabbable grabbable;

    [Tooltip("Button which toggles the switch")]
    public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;

    [Tooltip("Event triggered when switch is enabled")]
    public UnityEvent onEnabled;

    [Tooltip("Event triggered when switch is disabled")]
    public UnityEvent onDisabled;

    private bool toggled;

    public void Update()
    {
        var grabber = grabbable.GetGrabber();
        if (grabber == null)
        {
            return;
        }

        var controller = grabber.GetController();
        if (!OVRInput.GetDown(button, controller))
        {
            return;
        }

        toggled = !toggled;

        if (toggled)
        {
            onEnabled.Invoke();
        }
        else
        {
            onDisabled.Invoke();
        }
    }
}