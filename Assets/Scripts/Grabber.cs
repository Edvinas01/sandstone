using UnityEngine;
using UnityEngine.Events;

public class Grabber : OVRGrabber
{
    [Tooltip("Event called when the grabbed is grabbed")]
    public UnityEvent onGrab;

    [Tooltip("Event called when the grabbed is released")]
    public UnityEvent onRelease;

    private bool grabbed;

    public override void Update()
    {
        base.Update();

        if (!grabbed && m_grabbedObj != null)
        {
            onGrab.Invoke();
            grabbed = true;
        }
        
        if (grabbed && m_grabbedObj == null)
        {
            onRelease.Invoke();
            grabbed = false;
        }
    }

    public OVRInput.Controller GetController()
    {
        return m_controller;
    }
}