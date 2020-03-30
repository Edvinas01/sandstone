using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class KeyboardTrigger : MonoBehaviour
    {
        [Tooltip("The trigger key code")]
        public KeyCode keyCode;

        [Tooltip("Event called when trigger key is pressed")]
        public UnityEvent onKeyDown;

        [Tooltip("Event called when trigger key is released")]
        public UnityEvent onKeyUp;

        private bool isToggled;

        public void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                onKeyDown.Invoke();
            }
            else if (Input.GetKeyUp(keyCode))
            {
                onKeyUp.Invoke();
            }
        }
    }
}
