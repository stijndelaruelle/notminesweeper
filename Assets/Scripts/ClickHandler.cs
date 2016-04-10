using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class ClickHandler : MonoBehaviour
    {
        [SerializeField]
        private float m_DragMargin = 0.1f;
        private float m_StartDragPosition;
        private bool m_IsDragging = false;

        private bool m_AllowClick = true;

        private void Start()
        {
            GameManager.Instance.GameOverEvent += OnGameOver;
            GameManager.Instance.GameResetEvent += OnGameReset;
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameOverEvent -= OnGameOver;
                gameManager.GameResetEvent -= OnGameReset;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && m_AllowClick)
            {
                m_StartDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                m_IsDragging = false;
            }

            if (Input.GetMouseButton(0) && m_IsDragging == false)
            {
                float currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

                if (Mathf.Abs(currentPos - m_StartDragPosition) >= m_DragMargin)
                {
                    m_IsDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && m_AllowClick)
            {
                //Check if we were dragging, if so don't click!
                if (!m_IsDragging)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        IClickable clickableObject = hit.transform.gameObject.GetComponent<IClickable>();

                        if (clickableObject != null)
                        {
                            clickableObject.OnClick();
                        }
                    }
                }
            }
        }

        private void OnGameOver()
        {
            m_AllowClick = false;
        }

        private void OnGameReset()
        {
            m_AllowClick = true;
        }
    }
}
