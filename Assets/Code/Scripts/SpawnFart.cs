using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;

namespace Code.Scripts
{
   public class SpawnFart : MonoBehaviour
   {
      [SerializeField] private GameObject fart;
      [SerializeField] private Transform butt;
      private LogicScript _logicScript;
      [SerializeField] private InputActionReference _playerInput;
      private void Awake()
      {
         _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
         // _playerInput = GetComponent<InputActionReference>();
         // var fartAction = _playerInput.("Fart");
         _playerInput.action.performed += ctx => Farted(ctx);
         // inputAction += ctx => Farted(ctx);
      }
      // TODO retry using space
      // TODO add more instructions
      // TODO replace kattenpalen with more dangerous things
      // TODO increase speed every x palen
      // TODO give reward every x palen (extra balloon/fishy/?)
      private void Farted(object ctx)
      {
         Debug.Log("fart event");
         if (_logicScript.IsGameOver) return;
         Instantiate(fart, new Vector3(butt.position.x, butt.position.y, 0), butt.rotation);
      }

      // private void OnFart()
      // {
      //    if (_logicScript.IsGameOver) return;
      //    Instantiate(fart, new Vector3(butt.position.x, butt.position.y, 0), butt.rotation);
      // }
      
      void Start()
      {
         // playerInput = GetComponent<PlayerInput>();
         // _playerInput.FindAction("Fart", false) += ctx => OnActionTriggered(ctx);
      }

      // private void OnActionTriggered(InputAction.CallbackContext ctx)
      // {
      //    if (ctx.action.name == "fart")
      //    {
      //       if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() != null)
      //       {
      //          // Handle the fart action as a UI input.
      //          Debug.Log("Farted from UI");
      //       }
      //       else
      //       {
      //          // Handle the fart action as a non-UI input.
      //          Debug.Log("Farted from non-UI");
      //       }
      //    }
      // }

      // void OnDisable()
      // {
      //    fartAction.performed -= ctx => Fart();
      // }
   }
}
