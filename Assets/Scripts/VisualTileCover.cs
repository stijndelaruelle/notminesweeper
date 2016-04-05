using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class VisualTileCover : MonoBehaviour, IClickable
    {
        //Add event on fly away
        [SerializeField]
        private Animator m_Animator;

        private void Start()
        {
            //Randomize UV offset
        }

        public void OnClick()
        {
            //Trigger the flip animation
            m_Animator.SetTrigger("Trigger");
        }
    }
}
