using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [Header("Player Movement Settings")]

    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask obstacleLayer; 
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private InputActionReference moveAction;
    public bool CanMove { get; set; } = true; 
    #endregion

    #region Variables

    private Vector2 direction;

    #endregion

    #region Build-in Methods
    void Update()
    {
        direction = moveAction.action.ReadValue<Vector2>();
        direction = CalculateIsoMove(direction.x, direction.y);
    }

    void FixedUpdate()
    {
        if (!CanMove) return;
        MovePlayer();
    }

    #endregion

    #region Custom Methods
    /// <summary>
    ///
    /// </summary>
    private void MovePlayer()
    {
        Vector2 newPosition = playerRigidbody.position + direction * moveSpeed * Time.fixedDeltaTime;

        RaycastHit2D hit = Physics2D.Raycast(playerRigidbody.position, direction, moveSpeed * Time.fixedDeltaTime, obstacleLayer);
        if (hit.collider == null)
        {
            playerRigidbody.MovePosition(newPosition);
        }
    }

    /// <summary>
    ///
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
