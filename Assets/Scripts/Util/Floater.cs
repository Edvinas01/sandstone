using UnityEngine;

namespace Util
{
    public class Floater : MonoBehaviour
    {
        [Tooltip("The transform to float")]
        public Transform floater;

        [Tooltip("Which point to rotate around")]
        public Transform rotateAround;

        public Vector3 rotateAxis = Vector3.up;

        public float floatIntensity = 1f;

        public float rotateIntensity = 1f;

        private Vector3 initialPosition;

        private void Awake()
        {
            initialPosition = floater.position;
        }

        private void Update()
        {
            floater.RotateAround(
                rotateAround.position,
                rotateAxis,
                Time.deltaTime * rotateIntensity
            );

            var yOffset = Mathf.Sin(Time.time) * floatIntensity;

            var position = floater.position;

            floater.position = new Vector3(
                position.x,
                initialPosition.y + yOffset,
                position.z
            );
        }
    }
}
