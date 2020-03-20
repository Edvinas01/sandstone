using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Rules;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableSnap : MonoBehaviour
{
    [Tooltip("Destination transform where to snap the object")]
    public Transform destination;

    [Tooltip("Rules that must match in order for object to snap")]
    public List<Rule> rules;

    [Tooltip("Called when something enters the snap")]
    public UnityEvent onEnter;

    [Tooltip("Called when something exits the snap")]
    public UnityEvent onExit;

    [Tooltip("Called when something snaps in")]
    public UnityEvent onSnap;

    [Tooltip("Called when something snaps out")]
    public UnityEvent onUnsnap;

    private UnityAction snapAction;
    private UnityAction unsnapAction;

    private Grabbable trackedGrabbable;

    public void OnDrawGizmos()
    {
        if (trackedGrabbable == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            trackedGrabbable.transform.position,
            destination.transform.position
        );
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     var grabbable = GetGrabbable(other);
    //     if (grabbable != null && Accepts(grabbable))
    //     {
    //         trackedGrabbable = grabbable;
    //     }
    // }
    //
    // public void OnTriggerExit(Collider other)
    // {
    //     var grabable = GetGrabbable(other);
    //     if (grabable != null && grabable == trackedGrabbable)
    //     {
    //         trackedGrabbable = null;
    //     }
    // }

    public void Update()
    {
        if (trackedGrabbable != null && !trackedGrabbable.isGrabbed)
        {
            trackedGrabbable = null;
            onExit.Invoke();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var grabbable = GetGrabbable(other);
        if (grabbable != null && grabbable.isGrabbed)
        {
            if (trackedGrabbable == null || !trackedGrabbable.isGrabbed)
            {
                trackedGrabbable = grabbable;
                onEnter.Invoke();
            }
        }
    }

    // public void OnTriggerStay(Collider other)
    // {
    //     var grabbable = GetGrabbable(other);
    //     if (grabbable == null || !Accepts(grabbable))
    //     {
    //         return;
    //     }
    //
    //     trackedGrabbable = grabbable;
    //
    //     Debug.Log("New grabable: " + trackedGrabbable.name);
    //     unsnapAction = () =>
    //     {
    //         Debug.Log("Unsnap: " + trackedGrabbable.name);
    //         grabbable.onGrab.RemoveListener(unsnapAction);
    //         trackedGrabbable = null;
    //     };
    //
    //     snapAction = () =>
    //     {
    //         SnapToDestination(grabbable);
    //         grabbable.onRelease.RemoveListener(snapAction);
    //         grabbable.onGrab.AddListener(unsnapAction);
    //     };
    //     
    //     grabbable.onRelease.AddListener(snapAction);
    // }

    // public void OnTriggerEnter(Collider other)
    // {
    //     var grabbable = GetGrabbable(other);
    //     if (grabbable == null || !Accepts(grabbable))
    //     {
    //         return;
    //     }
    //
    //     SetTracked(grabbable);
    // }
    //
    // public void OnTriggerExit(Collider other)
    // {
    //     var grabbable = GetGrabbable(other);
    //     if (grabbable == null || trackedGrabbable != grabbable)
    //     {
    //         return;
    //     }
    //
    //     SetTracked(null);
    // }

    private void Track(Grabbable grabbable)
    {
        trackedGrabbable = grabbable;
        snapAction = CreateSnapAction(trackedGrabbable);
        unsnapAction = CreateUnsnapAction();

        trackedGrabbable.onRelease.AddListener(snapAction);
        trackedGrabbable.onGrab.AddListener(unsnapAction);

        onEnter.Invoke();
    }

    private void Untrack()
    {
        trackedGrabbable.onRelease.RemoveListener(snapAction);
        trackedGrabbable.onGrab.RemoveListener(unsnapAction);

        trackedGrabbable = null;
        unsnapAction = null;
        snapAction = null;

        onExit.Invoke();
    }

    private void SetTracked(Grabbable grabbable)
    {
        if (grabbable != null)
        {
            Track(grabbable);
        }
        else
        {
            Untrack();
        }
    }

    private UnityAction CreateSnapAction(Grabbable grabbable)
    {
        return () => { SnapToDestination(grabbable); };
    }

    private UnityAction CreateUnsnapAction()
    {
        return () => { onUnsnap.Invoke(); };
    }

    private void SnapToDestination(Component target)
    {
        var targetTransform = target.transform;

        targetTransform.position = destination.position;
        targetTransform.rotation = destination.rotation;
        targetTransform.parent = destination;

        var body = target.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.isKinematic = true;
        }
    }

    private bool Accepts(OVRGrabbable grabbable)
    {
        if (trackedGrabbable != null || !grabbable.isGrabbed)
        {
            return false;
        }

        return rules.All(rule => rule.Accepts(grabbable));
    }

    [CanBeNull]
    private static Grabbable GetGrabbable(Component other)
    {
        return other.gameObject.GetComponent<Grabbable>();
    }
}