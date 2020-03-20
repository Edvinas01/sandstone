using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : OVRGrabbable
{
    [Tooltip("Event called when the grabbable is grabbed")]
    public UnityEvent onGrab;

    [Tooltip("Event called when the grabbable is released")]
    public UnityEvent onRelease;

    private bool grabbed;

    public void Update()
    {
        if (!grabbed && isGrabbed)
        {
            onGrab.Invoke();
            grabbed = true;
        }
        
        if (grabbed && !isGrabbed)
        {
            onRelease.Invoke();
            grabbed = false;
        }
    }
    
    [CanBeNull]
    public Grabber GetGrabber()
    {
        return grabbedBy as Grabber;
    }
}