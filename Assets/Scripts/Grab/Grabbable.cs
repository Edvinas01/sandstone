using UnityEngine;
using UnityEngine.Events;

namespace Grab
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour
    {
        [Tooltip("Called when grabbable is deselected as valid for grabbing")]
        public UnityEvent onDeselected;

        [Tooltip("Called when grabbable is selected as valid for gabbing")]
        public UnityEvent onSelected;

        [HideInInspector]
        public Grabber grabbedBy;

        public Rigidbody Body { get; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            onDeselected.Invoke();
            grabbedBy = null;
        }
    }
}
