using UnityEngine;
using UnityEngine.Events;

namespace Door
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlidingDoor : MonoBehaviour
    {
        [Tooltip("Where the door should slide to (world)")]
        public Vector3 direction;

        [Tooltip("Distance to the destination where to door will snap")]
        public float snapThreshold = 0.01f;

        [Tooltip("Sliding speed")] public float speed = 1f;

        [Tooltip("Called when the door starts sliding")]
        public UnityEvent onStartSlide;

        [Tooltip("Called when the door ends sliding")]
        public UnityEvent onEndSlide;

        private new Collider collider;
        private Rigidbody body;
        private Vector3 destination;

        /// <summary>
        /// Start sliding.
        /// </summary>
        public void Slide()
        {
            var position = body.position;
            var normDirection = direction.normalized;
            var size = collider.bounds.size;

            destination = new Vector3(
                position.x + (normDirection.x * size.x),
                position.y + (normDirection.y * size.y),
                position.z + (normDirection.z * size.z)
            );

            onStartSlide.Invoke();
        }

        private void Awake()
        {
            collider = GetComponentInChildren<Collider>();
            body = GetComponent<Rigidbody>();
            destination = body.position;
        }

        private void FixedUpdate()
        {
            var position = body.position;
            if (position == destination)
            {
                return;
            }

            var newPosition = Vector3.MoveTowards(
                position,
                destination,
                Time.deltaTime * speed
            );

            var distance = Vector3.Distance(
                newPosition,
                destination
            );

            if (distance <= snapThreshold)
            {
                body.MovePosition(destination);
                onEndSlide.Invoke();
            }
            else
            {
                body.MovePosition(newPosition);
            }
        }
    }
}
