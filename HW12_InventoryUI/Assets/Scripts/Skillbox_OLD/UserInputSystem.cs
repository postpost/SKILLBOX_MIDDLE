using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

////for Input data collection
//public partial class UserInputSystem : SystemBase
//{
//    private EntityQuery _moveQuery;
//    private InputAction _moveAction;
//    private float2 _moveInput;

//    protected override void OnCreate()
//    {
//        base.OnCreate();
//        _moveQuery = GetEntityQuery(ComponentType.ReadOnly<UserInputSystem>());
//    }

//    protected override void OnStartRunning()
//    {
//        _moveAction = new InputAction("Move", binding: "<Gamepad>/rightStick");
//        _moveAction.AddCompositeBinding("Dpad")
//            .With("Up", "<Keyboard>/w")
//            .With("Down", "<Keyboard>/s")
//            .With("Left", "<Keyboard>/a")
//            .With("Right", "<Keyboard>/d");

//        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
//        _moveAction.started += context => { _moveInput = context.ReadValue<Vector2>(); };
//        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
//        _moveAction.Enable();
            
//    }

//    protected override void OnStopRunning()
//    {
//        _moveAction.Disable();
//    }

//    protected override void OnUpdate()
//    {
//        float deltaTime = SystemAPI.Time.DeltaTime;
//        foreach (var (transform, moveSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMoveSpeed>>())
//        {
//            transform.ValueRW.Position += new float3(_moveInput.x, 0, _moveInput.y)* moveSpeed.ValueRO.Value * deltaTime;
//        }
//    }
//}
