using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Entities;

[UpdateBefore(typeof(RotateSystem))]
public partial class InputSystem : SystemBase
{
    InputAction rotateAction;
    protected override void OnCreate()
    {
        rotateAction = new InputAction();
        rotateAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/LeftArrow")
            .With("Negative", "<Keyboard>/RightArrow");
        rotateAction.Enable();
    }

    protected override void OnUpdate()
    {
        var rotation = rotateAction.ReadValue<float>();
        Entities
        .WithName("InputSystem")
        .ForEach((ref RotateComponent rotate) =>
        {
            rotate.direction = rotation;
        }).ScheduleParallel();
    }
}
