using UnityEngine;

namespace Assets.Scripts.Client.PlayerInput
{
    public class CameraController : MonoBehaviour
    {
        void Update()
        {
            Vector3 newPosition = transform.position;
            newPosition.y += Input.mouseScrollDelta.y;

            if (newPosition.y > 5.85)
            {
                newPosition = new Vector3(newPosition.x, 5.85f, newPosition.z);
            }
            else if (newPosition.y < -5f)
            {
                newPosition = new Vector3(newPosition.x, -5, newPosition.z);
            }

            transform.position = newPosition;
        }
    }
}
