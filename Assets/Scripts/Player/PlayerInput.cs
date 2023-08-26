using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public FrameInput FrameInput { get; private set; }

    private void Update() => FrameInput = Gather();

    private FrameInput Gather()
    {
        return new FrameInput
        {
            DashDown = Input.GetKeyDown(KeyCode.Space),
            AttackDown = Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0),
            ShootDown = Input.GetMouseButtonDown(1),
            ShootHeld = Input.GetMouseButton(1),
            ShootUp = Input.GetMouseButtonUp(1),
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),

            WriteDown = Input.GetKeyDown(KeyCode.F),

            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition),
            ExampleActionHeld = Input.GetKey(KeyCode.E),
        };
    }
}

public struct FrameInput
{
    public Vector2 Move;
    public Vector3 MousePosition;
    public bool DashDown;
    public bool AttackDown;
    public bool ShootDown;
    public bool ShootHeld;
    public bool ShootUp;
    public bool WriteDown;
    public bool ExampleActionHeld;
}
