using Code.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class FartContoller : MonoBehaviour
{
    public InputActionAsset inputAsset;
    public GameObject logicManager;
    public GameObject spawnFart;
    public GameObject player;

    private InputActionMap _inputActionMap;
    private InputAction _fartAction;
    private bool _tapped;

    private void OnEnable()
    {
        _inputActionMap = inputAsset.FindActionMap("Player");
        _fartAction = _inputActionMap.FindAction("Fart");
        _fartAction.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void Update()
    {
        if (_fartAction.triggered)
        {
            Debug.Log("fart");
            HandleFart();
            return;
        }

        Debug.Log("touch count " + Touch.activeTouches.Count + ", tapped " + _tapped);
        if (Touch.activeTouches.Count > 0 && !_tapped)
        {
            Debug.Log("fart");
            _tapped = true;
            HandleFart();
        }

        if (_tapped && Touch.activeTouches.Count == 0)
        {
            Debug.Log("unfart");
            _tapped = false;
        }
    }

    private void HandleFart()
    {
        logicManager.GetComponent<LogicScript>().HandleFart();
        spawnFart.GetComponent<SpawnFart>().HandleFart();
        player.GetComponent<PlayerScript>().HandleFart();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        _fartAction.Disable();
    }
}