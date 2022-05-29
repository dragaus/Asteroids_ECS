using UnityEngine.InputSystem;
using Unity.Entities;

[UpdateBefore(typeof(RotateSystem))]
[UpdateBefore(typeof(MovementSystem))]
[UpdateBefore(typeof(ShootSystem))]
public partial class InputSystem : SystemBase
{
    InputAction rotateAction;
    InputAction moveAction;
    InputAction shootAction;
    bool lastFrameShoot;

    protected override void OnCreate()
    {
        rotateAction = new InputAction();
        moveAction = new InputAction();
        shootAction = new InputAction();

        rotateAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/LeftArrow")
            .With("Negative", "<Keyboard>/RightArrow");
        moveAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/UpArrow");
        shootAction.AddCompositeBinding("Axis")
            .With("Positive", "<Keyboard>/space");

        moveAction.Enable();
        rotateAction.Enable();
        shootAction.Enable();
    }

    protected override void OnUpdate()
    {
        float rotation = rotateAction.ReadValue<float>();
        float movementSpeed = moveAction.ReadValue<float>();
        float shoot = shootAction.ReadValue<float>();
        bool hasToShoot = shoot > 0;

        Entities
            .WithName("InputSystem")
            .ForEach((ref PlayerComponent playerComponent, ref RotateComponent rotate, ref MovementComponent movementComponent, ref ShooterComponent shooter) =>
            {
                movementComponent.movementSpeed = movementSpeed * movementComponent.baseMovementSpeed;
                rotate.direction = rotation;
                if(hasToShoot && shooter.canShoot)
                {
                    shooter.hasToShoot = true;
                }
                else
                {
                    shooter.hasToShoot = false;
                }
                shooter.canShoot = !hasToShoot && !shooter.hasToShoot;

            }).ScheduleParallel();
    }
}
