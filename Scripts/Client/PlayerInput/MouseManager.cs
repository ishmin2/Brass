using UnityEngine;

namespace Assets.Scripts.Client.PlayerInput
{
    public class MouseManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                {
                    Debug.Log("Click on: " + raycastHit.transform.name + " - parent: " + raycastHit.transform.parent.name);
                    var clickableObjects = raycastHit.collider.GetComponents<IClickable>();
                    foreach (var clickableObject in clickableObjects)
                        clickableObject.OnClick();
                }
            }
        }
    }
}
