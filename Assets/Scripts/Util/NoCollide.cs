using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class NoCollide : MonoBehaviour
    {
        [Tooltip("Game which should ignore collision with this game object")]
        public List<GameObject> gameObjects;

        private void Start()
        {
            var childColliders = GetComponentsInChildren<Collider>();
            foreach (var otherGameObject in gameObjects)
            {
                var otherColliders = otherGameObject.GetComponentsInChildren<Collider>();
                foreach (var childCollider in childColliders)
                {
                    foreach (var otherCollider in otherColliders)
                    {
                        Physics.IgnoreCollision(childCollider, otherCollider);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            foreach (var obj in gameObjects)
            {
                Gizmos.DrawLine(position, obj.transform.position);
            }
        }
    }
}
