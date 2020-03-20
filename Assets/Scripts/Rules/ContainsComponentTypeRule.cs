using System.Linq;

namespace Rules
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ContainsComponentTypeRule : Rule
    {
        [Tooltip("Component types to check against")]
        public List<string> typeNames;

        protected override bool Accepts(GameObject target)
        {
            return typeNames.Any(typeName => target.GetComponent(typeName) != null);
        }
    }
}