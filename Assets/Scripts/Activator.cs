using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Activator : MonoBehaviour
{
    public enum Mode
    {
        None,
        Activate,
        Deactivate
    }

    [Tooltip("Mode to start the activator with")]
    public Mode mode = Mode.None;

    [ReadOnly]
    [Tooltip("Current state of the activator")]
    public bool isActive;

    [Tooltip("Event fired when the activator is activated")]
    public UnityEvent onActivated;

    [Tooltip("Event fired when the activator is deactivated")]
    public UnityEvent onDeactivated;

    private bool isModeHandled;

    public void Update()
    {
        if (isModeHandled)
        {
            return;
        }

        HandleMode();
        isModeHandled = true;
    }

    public void Activate()
    {
        onActivated.Invoke();
        isActive = true;
    }

    public void Deactivate()
    {
        onDeactivated.Invoke();
        isActive = false;
    }

    private void HandleMode()
    {
        switch (mode)
        {
            case Mode.None:
                break;
            case Mode.Activate:
                Activate();
                break;
            case Mode.Deactivate:
                Deactivate();
                break;
        }
    }
}
