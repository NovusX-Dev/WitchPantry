using System;
using UnityEngine;
using WitchPantry.Data.Simulation;
using WitchPantry.Runtime.Simulation;

namespace WitchPantry.Core
{
    public sealed class GameLoopManager : MonoBehaviour
    {
        [SerializeField] private LoopData loopData;
        
        public SimulationLoop SimLoop { get; private set; }

        private void Awake()
        {
            var tickDurationSeconds = loopData != null ? loopData.TickDurationSeconds : 1f;
            var maxTicksPerAdvance = loopData != null ? loopData.MaxTicksPerAdvance : 5;
            SimLoop = new SimulationLoop(tickDurationSeconds, maxTicksPerAdvance);
            
            //Register all receivers
        }

        private void Update()
        {
            try
            {
                SimLoop.Advance(Time.deltaTime);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameLoopManager] Error advancing simulation loop:\n{e}");
            }
        }

        public void RegisterReceiver(ISimulationTickReceiver receiver) => SimLoop.Register(receiver);

        public void UnRegisterReceiver(ISimulationTickReceiver receiver) => SimLoop.Unregister(receiver);

        public void PauseSimulation() => SimLoop.Pause();
        
        public void ResumeSimulation() => SimLoop.Resume();
    }
}
