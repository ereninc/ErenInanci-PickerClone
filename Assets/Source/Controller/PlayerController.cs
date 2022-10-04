using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ControllerBaseModel
{
    [SerializeField] private PointerController pointerController;
    [Header("Movement")]
    public float ForwardSpeed;
    public float ExtraForwardSpeed;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float roadXLimit;
    [SerializeField] private float xPosition;
    [SerializeField] private float lastXPosition;
    [SerializeField] private float sensitive;
    [SerializeField] private Vector3 movePosition;
    private float xPos;
    private float xDiff;

    public override void Initialize()
    {
        base.Initialize();
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

    public void OnPointerDown()
    {
        lastXPosition = pointerController.PointerDownPosition.x;
    }

    public void OnPointer()
    {
        xDiff = pointerController.PointerPosition.x - lastXPosition;
        lastXPosition = pointerController.PointerPosition.x;
    }

    public void OnPointerUp(){ }

    private void movementUpdate()
    {
        xPos = transform.position.x + xDiff * Time.deltaTime * sensitive;
        xPos = Mathf.Clamp(xPos, -roadXLimit, roadXLimit);
        movePosition = new Vector3(xPos, transform.position.y, transform.position.z + (ForwardSpeed + ExtraForwardSpeed) * Time.fixedDeltaTime);
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