using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class GameObjectFilter : MonoBehaviour
    {
        [Tooltip("Game container which will trigger the filter")]
        public GameObjectContainer gameObjectContainer;

        [Tooltip("Game objects which will trigger the filter")]
        public List<GameObject> allowed;

        [Tooltip("Called when an allowed game object is filtered")]
        public GameObjectEvent onFilter;

        [Serializable]
        public class GameObjectEvent : UnityEvent<GameObject>
        {
        }

        public void Filter(GameObject obj)
        {
            foreach (var allowedObj in allowed)
            {
                if (obj == allowedObj || obj.transform.IsChildOf(allowedObj.transform))
                {
                    onFilter.Invoke(obj);
                }
            }
        }

        private void Awake()
        {
            if (gameObjectContainer == null)
            {
                return;
            }

            foreach (var obj in gameObjectContainer.gameObjects)
            {
                allowed.Add(obj);
            }
        }
    }
}
