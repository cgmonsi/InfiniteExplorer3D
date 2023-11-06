using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Controls the camera's position and movement, ensuring it follows the player with a smooth damping effect.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothSpeed = 0.125f;

        private void LateUpdate()
        {
            var desiredPosition = playerTransform.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(playerTransform);
        }
    }
}