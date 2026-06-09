using System.Collections.Generic;
using NUnit.Framework;
using WitchPantry.Data;
using WitchPantry.Runtime.Simulation;

namespace WitchPantry.Tests.EditMode.Runtime.Simulation
{
    public class SimulationLoopTests
    {
        [Test]
        public void Advance_AccumulatesPartialElapsedTimeUntilTickDurationIsReached()
        {
            var loop = new SimulationLoop();
            var receiver = new RecordingReceiver();
            loop.Register(receiver);

            var firstAdvanceTicks = loop.Advance(0.5f);
            var secondAdvanceTicks = loop.Advance(0.5f);

            Assert.That(firstAdvanceTicks, Is.EqualTo(0));
            Assert.That(secondAdvanceTicks, Is.EqualTo(1));
            Assert.That(loop.TickIndex, Is.EqualTo(1));
            Assert.That(receiver.Contexts, Has.Count.EqualTo(1));
            Assert.That(receiver.Contexts[0].TickIndex, Is.EqualTo(1));
            Assert.That(receiver.Contexts[0].TickDurationSeconds, Is.EqualTo(1f));
            Assert.That(receiver.Contexts[0].TotalSimulatedSeconds, Is.EqualTo(1d));
        }

        [Test]
        public void Advance_RunsMultipleTicksAndReportsDeterministicTickContext()
        {
            var loop = new SimulationLoop();
            var receiver = new RecordingReceiver();
            loop.Register(receiver);

            var ticksRun = loop.Advance(3.2f);

            Assert.That(ticksRun, Is.EqualTo(3));
            Assert.That(loop.TickIndex, Is.EqualTo(3));
            Assert.That(receiver.Contexts, Has.Count.EqualTo(3));
            Assert.That(receiver.Contexts[0].TotalSimulatedSeconds, Is.EqualTo(1d));
            Assert.That(receiver.Contexts[1].TotalSimulatedSeconds, Is.EqualTo(2d));
            Assert.That(receiver.Contexts[2].TotalSimulatedSeconds, Is.EqualTo(3d));
        }

        [Test]
        public void Advance_DoesNotRunTicksWhilePaused()
        {
            var loop = new SimulationLoop();
            var receiver = new RecordingReceiver();
            loop.Register(receiver);

            loop.Pause();
            var pausedTicks = loop.Advance(10f);
            loop.Resume();
            var resumedTicks = loop.Advance(1f);

            Assert.That(pausedTicks, Is.EqualTo(0));
            Assert.That(resumedTicks, Is.EqualTo(1));
            Assert.That(receiver.Contexts, Has.Count.EqualTo(1));
        }

        [Test]
        public void Advance_RunsReceiversByPhaseThenOrder()
        {
            var loop = new SimulationLoop();
            var calls = new List<string>();

            loop.Register(new RecordingReceiver("economy-10", calls, GlobalConstants.SimulationTickPhase.Economy, 10));
            loop.Register(new RecordingReceiver("production-20", calls, GlobalConstants.SimulationTickPhase.Production, 20));
            loop.Register(new RecordingReceiver("production-05", calls, GlobalConstants.SimulationTickPhase.Production, 5));

            loop.Advance(1f);

            Assert.That(calls, Is.EqualTo(new[]
            {
                "production-05",
                "production-20",
                "economy-10",
            }));
        }

        [Test]
        public void Register_IgnoresDuplicateReceivers()
        {
            var loop = new SimulationLoop();
            var receiver = new RecordingReceiver();

            loop.Register(receiver);
            loop.Register(receiver);
            loop.Advance(1f);

            Assert.That(receiver.Contexts, Has.Count.EqualTo(1));
        }

        [Test]
        public void UnRegister_RemovesReceiverFromFutureTicks()
        {
            var loop = new SimulationLoop();
            var receiver = new RecordingReceiver();

            loop.Register(receiver);
            loop.Advance(1f);
            loop.UnRegister(receiver);
            loop.Advance(1f);

            Assert.That(receiver.Contexts, Has.Count.EqualTo(1));
        }

        [Test]
        public void Advance_RespectsMaxTicksPerAdvance()
        {
            var loop = new SimulationLoop(tickDurationSeconds: 1f, maxTicksPerAdvance: 2);
            var receiver = new RecordingReceiver();
            loop.Register(receiver);

            var ticksRun = loop.Advance(10f);

            Assert.That(ticksRun, Is.EqualTo(2));
            Assert.That(loop.TickIndex, Is.EqualTo(2));
            Assert.That(receiver.Contexts, Has.Count.EqualTo(2));
        }

        private sealed class RecordingReceiver : ISimulationTickReceiver
        {
            private readonly string _id;
            private readonly List<string> _calls;

            public GlobalConstants.SimulationTickPhase Phase { get; }
            public int Order { get; }
            public List<SimulationTickContext> Contexts { get; } = new();

            public RecordingReceiver(
                string id = null,
                List<string> calls = null,
                GlobalConstants.SimulationTickPhase phase = GlobalConstants.SimulationTickPhase.Production,
                int order = 0)
            {
                _id = id;
                _calls = calls;
                Phase = phase;
                Order = order;
            }

            public void Tick(SimulationTickContext context)
            {
                Contexts.Add(context);
                if (_id != null)
                {
                    _calls?.Add(_id);
                }
            }
        }
    }
}
