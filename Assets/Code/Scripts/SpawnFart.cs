using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts
{
   public class SpawnFart : MonoBehaviour
   {
      [SerializeField] private GameObject fart;
      [SerializeField] private Transform butt;
      private LogicScript _logicScript;
      private void Awake()
      {
         _logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
      }

      private void OnFart()
      {
         if (_logicScript.IsGameOver) return;
         Instantiate(fart, new Vector3(butt.position.x, butt.position.y, 0), butt.rotation);
      }
   }
}
