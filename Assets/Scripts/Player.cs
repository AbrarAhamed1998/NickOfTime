using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

public class Player : MonoBehaviour, IPlayerActions
{
    [SerializeField] private Rigidbody2D playerRigidbody;

    protected PlayerControls _playerControl;

    protected Action moveAction, jumpAction, lookAction;

    private Vector2 _moveDirection, _lookDirection; 


	private void OnEnable()
	{
		_playerControl = new PlayerControls();
        _playerControl.Player.SetCallbacks(this);
        _playerControl.Player.Move.Enable();
        _playerControl.Player.Look.Enable();
	}

	private void OnDisable()
	{
        _playerControl.Player.Move.Disable();
        _playerControl.Player.Look.Disable();
    }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnMove(InputAction.CallbackContext context)
	{
        _moveDirection = context.ReadValue<Vector2>();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
        _lookDirection = context.ReadValue<Vector2>();
    }

    public void MovePlayer()
	{
        moveAction?.Invoke();
	}

    public void PlayerLook()
	{
        lookAction?.Invoke();
	}

    public void PlayerJump()
	{
        jumpAction?.Invoke();
	}

    private void RegisterControlEvents()
	{
        moveAction = () => { 
            
        };
	}
}
