namespace WitchPantry.Runtime.Simulation
{
    public readonly struct SimulationTickContext
    {
        public int TickIndex { get; }
        public float TickDurationSeconds { get; }
        public double TotalSimulatedSeconds { get; }
        
        public SimulationTickContext(int tickIndex, float tickDurationSeconds, double totalSimulatedSeconds)
        {
            TickIndex = tickIndex;
            TickDurationSeconds = tickDurationSeconds;
            TotalSimulatedSeconds = totalSimulatedSeconds;
        }
        
        public override string ToString()
        {
            return $"Tick {TickIndex} - Duration: {TickDurationSeconds}s, Total Simulated: {TotalSimulatedSeconds}s";
        }
    }
}