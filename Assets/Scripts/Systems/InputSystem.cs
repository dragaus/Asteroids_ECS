using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;

[UpdateBefore(typeof(RotateSystem))]
[UpdateBefore(typeof(MovementSystem))]
public partial class InputSystem : SystemBase
{
    InputAction rotateAction;
    InputAction moveAction;

    protected override void OnCreate()
    {
        rotateAction = new InputAction();
        moveAction = new InputAction();

        rotateAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/LeftArrow")
            .With("Negative", "<Keyboard>/RightArrow");
        moveAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/UpArrow");

        moveAction.Enable();
        rotateAction.Enable();
    }

    protected override void OnUpdate()
    {
        var rotation = rotateAction.ReadValue<float>();
        var movementSpeed = moveAction.ReadValue<float>();
        Entities
        .WithName("InputSystem")
        .ForEach((ref PlayerComponent playerComponent, ref RotateComponent rotate, ref MovementComponent movementComponent) =>
        {
            movementComponent.movementSpeed = movementSpeed * movementComponent.baseMovementSpeed;
            rotate.direction = rotation;
        }).ScheduleParallel();
    }
}
