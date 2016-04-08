using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public delegate void VoidDelegate();

    public class VisualTileCover : MonoBehaviour, IClickable
    {
        [SerializeField]
        private MeshRenderer m_MeshRenderer;

        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private Color m_EnabledColor;

        [SerializeField]
        private Color m_DisabledColor;

        private bool m_IsClickable;

        //Events
        private VoidDelegate m_TriggerEvent;
        public VoidDelegate TriggerEvent
        {
            get { return m_TriggerEvent; }
            set { m_TriggerEvent = value; }
        }

        private void Start()
        {
            //Randomize UV offset
        }

        public void OnClick()
        {
            if (m_IsClickable)
                TriggerAnimation();
        }

        public void TriggerAnimation()
        {
            //Trigger the flip animation
            m_Animator.SetTrigger("Trigger");
            m_Animator.ResetTrigger("Reset");

            if (m_TriggerEvent != null)
                m_TriggerEvent();

            StartCoroutine(DisableRoutine());
        }

        public void SetEnabled(bool state)
        {
            m_IsClickable = state;

            //Make nicer
            if (state == false)
                m_MeshRenderer.material.color = m_DisabledColor;
            else
                m_MeshRenderer.material.color = m_EnabledColor;
        }

        public void Reset()
        {
            m_Animator.SetTrigger("Reset");
            SetEnabled(false);
        }

        private IEnumerator DisableRoutine()
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }
    }
}
