using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;

    bool isMoving = false;
    float elapsedTime = 0f;
    Vector3 initialPos;
    Vector3 targetPos;
    [SerializeField] float moveTime;
    [SerializeField] GameObject playerModel;
    Vector3 moveInput;
    Vector3 move;
    [SerializeField] float deadzone;
    HeatManager heatManager;

    [Header("â∑ìx")]
    public float temperature;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        heatManager = GameObject.Find("BlockManager").GetComponent<HeatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // à⁄ìÆ
        moveInput = moveAction.ReadValue<Vector2>();
        moveInput = new Vector3(moveInput.x, 0, moveInput.y);
        if (!isMoving)
        {
            if (moveInput.x > deadzone)
            {
                move = Vector3.right;
            }
            else if (moveInput.x < -deadzone)
            {
                move = Vector3.left;
            }
            else if (moveInput.z > deadzone)
            {
                move = Vector3.forward;
            }
            else if (moveInput.z < -deadzone)
            {
                move = Vector3.back;
            }
            else
            {
                move = Vector3.zero;
            }

            if (move != Vector3.zero)
            {
                playerModel.transform.rotation = Quaternion.LookRotation(move);
            }
            Physics.Raycast(transform.position, move, out RaycastHit hit, 1f);
            if (hit.collider != null && hit.collider.tag == "Obstacle")
            {
                move = Vector3.zero;
            }
        }

        if (move.magnitude > 0.1f && !isMoving)
        {
            isMoving = true;
            elapsedTime = 0f;
            initialPos = transform.position;
            targetPos = initialPos + move;
            targetPos = SnapToGrid(targetPos);
            Debug.Log("Target Pos: " + targetPos);
        }
        else if (elapsedTime >= moveTime)
        {
            isMoving = false;
        }

        if (elapsedTime < moveTime && isMoving)
        {
            transform.position = Vector3.Lerp(initialPos, targetPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= moveTime)
            {
                transform.position = targetPos;
            }
        }

        //ç°Ç¢ÇÈèÍèäÇâ¡îM
        heatManager.heatGrid[(int)SnapToGrid(targetPos).x, (int)SnapToGrid(targetPos).z] += temperature;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x);
        float y = Mathf.Round(position.y);
        float z = Mathf.Round(position.z);
        return new Vector3(x, y, z);
    }
}
