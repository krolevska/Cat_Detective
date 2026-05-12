using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [Header("Player Movement Settings")]

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayer; // Шар для перешкод
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] public InputActionReference moveAction; // Посилання на дію руху
    #endregion

    #region Variables

    private Vector2 direction; // Напрямок руху гравця

    #endregion

    #region Build-in Methods
    void Update()
    {
        direction = moveAction.action.ReadValue<Vector2>();
        CalculateIsoMove(direction.x, direction.y);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    #endregion

    #region Custom Methods
    /// <summary>
    /// Рух гравця з перевіркою на зіткнення
    /// </summary>
    private void MovePlayer()
    {
        Vector2 newPosition = playerRigidbody.position + direction * moveSpeed * Time.fixedDeltaTime;

        // Перевірка на зіткнення з перешкодами
        RaycastHit2D hit = Physics2D.Raycast(playerRigidbody.position, direction, moveSpeed * Time.fixedDeltaTime, obstacleLayer);
        if (hit.collider == null)
        {
            playerRigidbody.MovePosition(newPosition);
        }
    }

    /// <summary>
    /// Перетворення руху на ізометричний
    /// </summary>
    /// <param name="moveX"></param>
    /// <param name="moveY"></param>
    /// <returns></returns>
    private Vector2 CalculateIsoMove(float moveX, float moveY)
    {
        
        Vector2 isoMove = new Vector2(moveX - moveY, (moveX + moveY) / 2).normalized;
        direction = isoMove;
        return isoMove;
    }
    #endregion
}
