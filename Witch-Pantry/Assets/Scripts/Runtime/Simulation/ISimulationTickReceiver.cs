using WitchPantry.Data;

namespace WitchPantry.Runtime.Simulation
{
    public interface ISimulationTickReceiver
    {
        public GlobalConstants.SimulationTickPhase Phase { get; }
        public int Order { get; }

        public void Tick(SimulationTickContext context);
    }
}
