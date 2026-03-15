using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction attackAction;
    CharacterController controller;

    private Vector3 _direction = Vector3.zero;
    private Vector2 _rotation = Vector3.zero;

    public float movingSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public float ThrowForce = 1.0f;

    public GameObject goToThrow = null;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        attackAction = InputSystem.actions.FindAction("Attack");

        Debug.Log(Physics.gravity.y);
    }

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        if (controller.isGrounded)
        {
            // another way:
            //_direction = transform.right * moveValue.x + transform.forward * moveValue.y;
            _direction = transform.TransformDirection(moveValue.x, 0, moveValue.y);
            _direction *= movingSpeed;
            if (jumpAction.IsPressed())
            {
                _direction.y = 8.0f;
            }
            else
            {
                // avoid to "jump" when look up
                _direction.y = 0.0f;
            }
        }
        _direction.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(_direction * Time.deltaTime);

        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        _rotation.x -= lookValue.y * rotationSpeed * Time.deltaTime;
        _rotation.y += lookValue.x * rotationSpeed * Time.deltaTime;
        _rotation.x = Mathf.Clamp(_rotation.x, -90, 90);
        transform.rotation = Quaternion.Euler(_rotation);

        if (attackAction.WasPressedThisFrame())
        {
            GameObject go = Instantiate(goToThrow,transform.position,Quaternion.identity);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * ThrowForce);
        }
    }
}
