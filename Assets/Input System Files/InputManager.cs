using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class InputManager : MonoBehaviour
{
    #region Components

    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InputManager>();
            }

            return instance;
        }
    }

    private UserInputs gameInputs;

    #endregion

    #region Unity Components

    private void Awake() => Setup();

    private void OnEnable() => GameInputState(true);

    private void OnDisable() => GameInputState(false);

    #endregion

    #region Main

    private void Setup()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }

        gameInputs = new UserInputs();
    }

    private void GameInputState(bool enabled)
    {
        if(enabled)
        {
            gameInputs.Enable();
        }
        else
        {
            gameInputs.Disable();
        }
    }

    #endregion

    #region Vector Inputs

    public Vector2 ScreenDrag()
    {
        return gameInputs.UI.ScreenDrag.ReadValue<Vector2>();
    }

    #endregion

}
