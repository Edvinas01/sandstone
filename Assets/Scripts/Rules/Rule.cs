namespace Rules
{
    using UnityEngine;

    public abstract class Rule : MonoBehaviour
    {
        public bool Accepts(object target)
        {
            GameObject obj = null;
            if (target is GameObject targetGameObject)
            {
                obj = targetGameObject;
            }

            if (target is Component targetComponent)
            {
                obj = targetComponent.gameObject;
            }

            if (obj != null && obj.activeSelf && obj.activeInHierarchy)
            {
                return Accepts(obj);
            }

            return false;
        }

        protected abstract bool Accepts(GameObject target);
    }
}