using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class FootStepTracker : MonoBehaviour
    {
        [Tooltip("Player whose foot steps should be tracked")]
        public CharacterController player;

        [Tooltip("Distance that a player has to move in order to register a foot step")]
        public float interval = 0.5f;

        [Tooltip("Event called when a left foot step is taken")]
        public UnityEvent onFootStepLeft;

        [Tooltip("Event called when a right foot step is taken")]
        public UnityEvent onFootStepRight;

        private Vector3 lastPos = Vector3.zero;
        private float distanceMoved;
        private bool left;

        public void Start()
        {
            if (player == null)
            {
                player = GetComponent<CharacterController>();
            }
        }

        public void FixedUpdate()
        {
            if (!player.isGrounded)
            {
                return;
            }

            var playerPos = player.transform.position;

            distanceMoved += Vector3.Distance(lastPos, playerPos);
            lastPos = playerPos;

            if (distanceMoved < interval)
            {
                return;
            }

            if (left)
            {
                onFootStepLeft.Invoke();
            }
            else
            {
                onFootStepRight.Invoke();
            }

            distanceMoved = 0f;
            left = !left;
        }
    }
}
