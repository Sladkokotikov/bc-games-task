using UnityEngine;

namespace DrawAndRun
{
    public class Spawnable: MonoBehaviour
    {
         public GameController Controller { get; protected set; }

         public void Init(GameController controller)
         {
             Controller = controller;
         }
    }
}