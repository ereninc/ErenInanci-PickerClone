using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBaseModel
{
    public static PlayerController Instance;
    [SerializeField] private PointerController pointerController;
    [Header("Movement")]
    public float ForwardSpeed;
    public float ExtraForwardSpeed;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float roadXLimit;
    [SerializeField] private float sensitive;
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float xPos;
    private float lastXPosition;
    private float xDiff;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        ExtraForwardSpeed = 0;
    }

    public override void ControllerUpdate(GameStates currentState)
    {
        base.ControllerUpdate(currentState);
        if (currentState == GameStates.Game)
        {
            pointerController.ControllerUpdate();
        }
    }

    public void OnLevelCompleted()
    {
        ForwardSpeed = 0f;
        ExtraForwardSpeed = 0f;
    }

    public void OnNextLevel()
    {
        ForwardSpeed = 5f;
        ExtraForwardSpeed = 0f;
    }

    public void OnPointerDown()
    {
        lastXPosition = pointerController.PointerDownPosition.x;
    }

    public void OnPointer()
    {
        xDiff = pointerController.PointerPosition.x - lastXPosition;
        lastXPosition = pointerController.PointerPosition.x;
    }

    public void OnPointerUp() 
    {

    }

    public void OnEnterPassArea() 
    {
        ForwardSpeed = 0f;
        ExtraForwardSpeed = 0f;
    }

    public void OnExitPassArea() 
    {
        ForwardSpeed = 5f;
        ExtraForwardSpeed = 0f;
    }

    private void movementUpdate()
    {
        xPos = transform.position.x + xDiff * Time.deltaTime * sensitive;
        xPos = Mathf.Clamp(xPos, -roadXLimit, roadXLimit);
        movePosition = new Vector3(xPos, 0.8f, 
            transform.position.z + (ForwardSpeed + ExtraForwardSpeed) * Time.fixedDeltaTime);
        rigid.MovePosition(movePosition);
    }

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameStates.Game)
        {
            movementUpdate();
        }
    }
}