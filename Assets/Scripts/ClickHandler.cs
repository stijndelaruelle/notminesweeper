using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class ClickHandler : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
}
