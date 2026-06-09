using System;
using System.Collections.Generic;

namespace WitchPantry.Runtime.Simulation
{
    public sealed class SimulationLoop
    {
        private readonly List<ISimulationTickReceiver> _receivers = new();

        private float _accumulatedSeconds;

        public float TickDurationSeconds { get; }
        public int MaxTicksPerAdvance { get; }
        public int TickIndex { get; private set; }
        public bool IsPaused { get; private set; }

        public SimulationLoop(float tickDurationSeconds = 1f, int maxTicksPerAdvance = 5)
        {
            if (tickDurationSeconds <= 0f) throw new ArgumentOutOfRangeException(nameof(tickDurationSeconds));
            if (maxTicksPerAdvance <= 0) throw new ArgumentOutOfRangeException(nameof(maxTicksPerAdvance));

            TickDurationSeconds = tickDurationSeconds;
            MaxTicksPerAdvance = maxTicksPerAdvance;
        }

        public void Register(ISimulationTickReceiver receiver)
        {
            if (receiver == null) throw new ArgumentNullException(nameof(receiver));
            if (_receivers.Contains(receiver)) return;
            _receivers.Add(receiver);
            _receivers.Sort(CompareReceivers);
        }

        public void UnRegister(ISimulationTickReceiver receiver)
        {
            Unregister(receiver);
        }

        public void Unregister(ISimulationTickReceiver receiver)
        {
            if (!_receivers.Contains(receiver))
            {
                return;
            }
            _receivers.Remove(receiver);
        }

        public void Pause()
        {
            IsPaused = true;
        }
        
        public void Resume()
        {
            IsPaused = false;
        }

        public int Advance(float elapsedSeconds)
        {
            if (IsPaused || elapsedSeconds <= 0) return 0;

            _accumulatedSeconds += elapsedSeconds;
            var maxAccumulatedSecond = TickDurationSeconds * MaxTicksPerAdvance;
            _accumulatedSeconds = Math.Min(_accumulatedSeconds, maxAccumulatedSecond);
            var tickRun = 0;
            while (_accumulatedSeconds >= TickDurationSeconds && tickRun < MaxTicksPerAdvance)
            {
                RunSingleTick();
                _accumulatedSeconds -= TickDurationSeconds;
                tickRun++;
            }

            return tickRun;
        }

        private void RunSingleTick()
        {
            TickIndex++;
            
            var context = new SimulationTickContext(TickIndex, TickDurationSeconds, TickIndex * TickDurationSeconds);
            foreach (var receiver in _receivers)
            {
                receiver.Tick(context);
            }
        }
        
        private static int CompareReceivers(ISimulationTickReceiver a, ISimulationTickReceiver b)
        {
            var phaseComparison = a.Phase.CompareTo(b.Phase);
            if (phaseComparison != 0) return phaseComparison;

            return a.Order.CompareTo(b.Order);
        }
    }
}
