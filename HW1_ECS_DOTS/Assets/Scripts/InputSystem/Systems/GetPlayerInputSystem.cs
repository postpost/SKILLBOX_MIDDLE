using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

//[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)] //update at the end of the initialization
public partial class GetPlayerInputSystem : SystemBase
{
    private NewActions inputAction;
    private Entity _playerEntity;

    protected override void OnCreate()
    {
        //эти компоненты дб у сущности (то, что мы вешаем на объект и то, что имеет Baker)
        //дожидаемя инициализации хотя бы одной сущности с этими компонентами
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<PlayerMoveInput>();
        inputAction = new NewActions();
    }

    protected override void OnStartRunning()
    {
        inputAction.Enable();
        inputAction.Player.Fire.performed += OnPlayerShoot;
        inputAction.Player.Dash.performed += OnPlayerDash;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
    }
    protected override void OnUpdate()
    {
        // 1. Читаем ввод
        var curMovement = inputAction.Player.Move.ReadValue<Vector2>();

        // 2. ВАЖНО: Дожидаемся завершения всех задач, которые используют этот компонент
        this.Dependency.Complete();

        // 3. Теперь можно безопасно записывать
        SystemAPI.SetSingleton(new PlayerMoveInput { Value = curMovement });
    }

    protected override void OnStopRunning()
    {
        inputAction.Player.Fire.performed -= OnPlayerShoot;
        inputAction.Player.Dash.performed -= OnPlayerDash;
        inputAction.Disable();
        _playerEntity = Entity.Null;
    }

    private void OnPlayerShoot(InputAction.CallbackContext context)
    {
        if (!SystemAPI.Exists(_playerEntity)) return;
        SystemAPI.SetComponentEnabled<FireProjectileTag>(_playerEntity, true);
        
    }

    private void OnPlayerDash(InputAction.CallbackContext context)
    {
        if(!SystemAPI.Exists(_playerEntity)) return;
        SystemAPI.SetComponentEnabled<PlayerDashTag>(_playerEntity, true); 
    }
}
