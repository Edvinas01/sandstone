using System.Collections.Generic;
using System.Linq;
using Rules;
using UnityEngine;

// TODO: Broken when two different objects are up close.
public class CollisionFade : MonoBehaviour
{
    [Tooltip("Screen fade which will be controller by this script")]
    public OVRScreenFade ovrScreenFade;

    [Tooltip("Radius of the inner fade sphere, where screen will be completely faded out")]
    public float fadeRadius = 0.15f;

    [Tooltip("Rules that have to be valid in order for fade to take affect")]
    public List<Rule> rules;

    private SphereCollider sphereCollider;
    private List<Collider> colliders;

    private float minDistance;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fadeRadius);
    }

    public void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        colliders = new List<Collider>();
    }

    public void Update()
    {
        if (colliders.Count > 0)
        {
            ovrScreenFade.SetFadeLevel(GetFadeLevel());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Accepts(other))
        {
            colliders.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (Accepts(other))
        {
            colliders.Remove(other);

            if (colliders.Count == 0)
            {
                ovrScreenFade.SetFadeLevel(0f);
            }
        }
    }

    private bool Accepts(Component target)
    {
        return rules.All(rule => rule.Accepts(target.gameObject));
    }

    private float GetDistance()
    {
        var position = sphereCollider.transform.position;
        var distance = float.MaxValue;

        var colPositions = colliders
            .Select(col => col.transform.position);

        foreach (var colPosition in colPositions)
        {
            if (Physics.Linecast(position, colPosition, out var hit))
            {
                distance = Mathf.Min(
                    Vector3.Distance(position, hit.point),
                    distance
                );
            }
            else
            {
                distance = 0;
            }
        }

        return distance;
    }

    private float GetFadeLevel()
    {
        var curFade = GetDistance() - fadeRadius;
        var maxFade = sphereCollider.radius - fadeRadius;

        var fade = Mathf.Clamp(
            curFade,
            0f,
            maxFade
        );

        return 1 - fade / maxFade;
    }
}