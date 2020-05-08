using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class GameObjectContainer : MonoBehaviour
    {
        public List<GameObject> gameObjects;

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
