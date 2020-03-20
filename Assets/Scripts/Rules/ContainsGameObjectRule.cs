namespace Rules
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ContainsGameObjectRule : Rule
    {
        [Tooltip("Game objects to check against")]
        public List<GameObject> gameObjects;

        protected override bool Accepts(GameObject target)
        {
            return gameObjects.Contains(target);
        }
    }
}