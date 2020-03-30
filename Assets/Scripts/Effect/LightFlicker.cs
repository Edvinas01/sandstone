using System.Collections.Generic;
using UnityEngine;
using Util;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

namespace Effect
{
    /// <summary>
    /// Component which will flicker a linked light while active by changing its
    /// intensity between the min and max values given. The flickering can be
    /// sharp or smoothed depending on the value of the smoothing parameter.
    ///
    /// Just activate / deactivate this component as usual to pause / resume flicker
    /// </summary>
    public class LightFlicker : MonoBehaviour
    {
        [Tooltip(
            "External light to flicker; you can leave this null if you attach script to a light"
        )]
        public new Light light;

        [Tooltip("Minimum random light intensity")]
        public float minIntensity;

        [Tooltip("Maximum random light intensity")]
        public float maxIntensity = 1f;

        [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
        [Range(1, 50)]
        public int smoothing = 5;

        // Continuous average calculation via FIFO queue
        // Saves us iterating every time we update, we just change by the delta
        private Queue<float> smoothQueue;
        private float lastSum;

        /// <summary>
        /// Reset the randomness and start again. You usually don't need to call
        /// this, deactivating/reactivating is usually fine but if you want a strict
        /// restart you can do.
        /// </summary>
        public void Reset()
        {
            smoothQueue.Clear();
            lastSum = 0;
        }

        public void Start()
        {
            smoothQueue = new Queue<float>(smoothing);
            // External or internal light?
            if (light == null)
            {
                light = GetComponent<Light>();
            }
        }

        public void Update()
        {
            if (light == null)
                return;

            // pop off an item if too big
            while (smoothQueue.Count >= smoothing)
            {
                lastSum -= smoothQueue.Dequeue();
            }

            // Generate random new item, calculate new average
            var newVal = RandomUtils.GetRandom(minIntensity, maxIntensity);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average
            light.intensity = lastSum / smoothQueue.Count;
        }
    }
}
