/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System;
using UnityEngine;
using OVRTouchSample;

namespace OculusSampleFramework
{
    public class DistanceGrabbable : OVRGrabbable
    {
        public string m_materialColorField = "Outline_Color";
        public Renderer m_renderer;
        
        [Tooltip("Should grabbable move event be ignored")]
        public bool ignoreMoveEvent;

        GrabbableCrosshair m_crosshair;
        GrabManager m_crosshairManager;
        MaterialPropertyBlock m_mpb;

        private Color initialColor;

        public bool InRange
        {
            get { return m_inRange; }
            set
            {
                m_inRange = value;
                RefreshCrosshair();
            }
        }
        bool m_inRange;

        public bool Targeted
        {
            get { return m_targeted; }
            set
            {
                m_targeted = value;
                RefreshCrosshair();
            }
        }
        bool m_targeted;

        protected override void Start()
        {
            base.Start();
            m_crosshair = gameObject.GetComponentInChildren<GrabbableCrosshair>();
            m_renderer = GetRenderer();
            m_crosshairManager = FindObjectOfType<GrabManager>();
            m_mpb = new MaterialPropertyBlock();
            RefreshCrosshair();
            initialColor = m_mpb.GetColor(m_materialColorField);

            if (m_renderer == null)
            {
                return;
            }
            
            m_renderer.SetPropertyBlock(m_mpb);
        }

        void RefreshCrosshair()
        {
            if (m_crosshair)
            {
                if (isGrabbed) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else if (!InRange) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else m_crosshair.SetState(Targeted ? GrabbableCrosshair.CrosshairState.Targeted : GrabbableCrosshair.CrosshairState.Enabled);
            }
            if (m_materialColorField != null)
            {
                if (m_renderer == null)
                {
                    return;
                }
                
                m_renderer.GetPropertyBlock(m_mpb);
                if (isGrabbed || !InRange) m_mpb.SetColor(m_materialColorField, initialColor);
                else if (Targeted) m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);
                else m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorInRange);
                m_renderer.SetPropertyBlock(m_mpb);
            }
        }

        public void SetColor(Color focusColor)
        {
            SetRendererColor(focusColor);
        }

        public void ClearColor()
        {
            SetRendererColor(initialColor);
        }
        
        private Renderer GetRenderer()
        {
            return m_renderer != null ? m_renderer : GetComponent<Renderer>();
        }

        private void SetRendererColor(Color color)
        {
            if (m_renderer == null)
            {
                return;
            }
            
            m_mpb.SetColor(m_materialColorField, color);
            m_renderer.SetPropertyBlock(m_mpb);
        }
    }
}
