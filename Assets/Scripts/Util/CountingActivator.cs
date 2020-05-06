using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class CountingActivator : MonoBehaviour
    {
        [Tooltip("After how many increments the activator should fire")]
        public int activationCount;

        [Tooltip("Called when activator is activated")]
        public UnityEvent onActivated;

        [Tooltip("Called when activator is deactivated")]
        public UnityEvent onDeactivated;

        private int count;

        public void Increment()
        {
            if (activationCount == count)
            {
                return;
            }

            if (activationCount == count + 1)
            {
                onActivated.Invoke();
            }

            count++;
        }

        public void Decrement()
        {
            if (activationCount == 0)
            {
                return;
            }

            if (activationCount == count)
            {
                onDeactivated.Invoke();
            }

            count--;
        }
    }
}
