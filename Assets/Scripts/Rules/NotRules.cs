namespace Rules
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;

    public class NotRules : Rule
    {
        [Tooltip("Rules to negate")] public List<Rule> rules;

        protected override bool Accepts(GameObject target)
        {
            return rules.All(rule => !rule.Accepts(target));
        }
    }
}