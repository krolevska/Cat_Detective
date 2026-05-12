using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Ціль, за якою слідкує камера
    [SerializeField] private Vector3 offset; // Зсув камери відносно цілі
    [SerializeField] private float smoothSpeed = 0.125f; // Швидкість згладжування руху камери
    private void LateUpdate()
    {
        Vector3 desiredPosition;
        desiredPosition.x = target.position.x + offset.x;
        desiredPosition.y = target.position.y + offset.y;
        desiredPosition.z = this.transform.position.z; // Зберігаємо поточну позицію по осі Z
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition + offset, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
