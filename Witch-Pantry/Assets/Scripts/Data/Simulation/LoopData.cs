using UnityEngine;

namespace WitchPantry.Data.Simulation
{
    [CreateAssetMenu(fileName = "Loop Data", menuName = "WitchPantry/Simulation/Loop Data", order = 0)]
    public class LoopData : ScriptableObject
    {
        [SerializeField] private float tickDurationSeconds = 1f;
        [SerializeField] private int maxTicksPerAdvance = 5;
        
        public float TickDurationSeconds => tickDurationSeconds;
        public int MaxTicksPerAdvance => maxTicksPerAdvance;
        
    }
}