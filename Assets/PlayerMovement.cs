using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 9f;
    public float rotationSpeed = 90.0f; // degrees per second
    public float maxAngle = 50.0f;
    public Rigidbody rigidBody;
    public Inputs inputs;
    public Collider hitBox;
    public Transform body;
    public Transform XAxis;
    public Transform YAxis;

    [Header("CharacterState")]
    public string stateValue;

    [Header("Input Actions")]
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction lookAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Player/Move");
        lookAction = InputSystem.actions.FindAction("Player/Look");

        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        inputs = new Inputs(new Vector2(0, 0), false);
    }

    void Update()
    {
        inputs.move = moveAction.ReadValue<Vector2>();
        inputs.look = lookAction.ReadValue<Vector2>();
        if (jumpAction.triggered)
        {
            inputs.jump = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       

        // Rotation
        Vector3 nextRotation = transform.eulerAngles + new Vector3(inputs.look.y * Time.fixedDeltaTime , inputs.look.x * Time.fixedDeltaTime * rotationSpeed,  0);
        if (Mathf.Sin(Mathf.Deg2Rad * nextRotation.x) > Mathf.Sin(Mathf.Deg2Rad * maxAngle))
            nextRotation.x = maxAngle;
        if (Mathf.Sin(Mathf.Deg2Rad * nextRotation.x) < Mathf.Sin(Mathf.Deg2Rad * -maxAngle))
            nextRotation.x = -maxAngle;
        
        // Movement
        Vector3 nextPosition = transform.position;
        Vector3 moveDirection = body.right * inputs.move.x + body.forward * inputs.move.y;
        moveDirection += (inputs.jump ? 1 : 0) * Vector3.up ;
        nextPosition += moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(nextPosition);
        rigidBody.MoveRotation(Quaternion.Euler(nextRotation));
        inputs.Reset();
    }
}
public struct Inputs
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public Inputs(Vector2 vec, bool ju)
    {
        move = vec;
        jump = ju;
        look = new Vector2(0,0);
    }
    public void Reset()
    {
        move = new Vector2(0, 0);
        jump = false;
    }
}