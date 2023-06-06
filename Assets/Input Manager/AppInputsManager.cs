using UnityEngine;
using UnityEngine.InputSystem;

public class AppInputsManager : MonoBehaviour
{
    #region Components

    static AppInputsManager _instance;

    public static AppInputsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AppInputsManager>();

            return _instance;
        }
    }

    AppInputs inputActions;

    #endregion

    #region Unity Callbacks

    void Awake() => Setup();

    void Start() => Init();

    #endregion

    #region Main

    void Setup()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Init()
    {
        inputActions = new AppInputs();

        inputActions.Enable();
    }

    public bool OnDoubleTap()
    {
        return inputActions.TouchInputs.DoubleTap.triggered;
    }

    #endregion
}
