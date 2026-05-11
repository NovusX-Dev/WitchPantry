using NUnit.Framework;
using WitchPantry.Runtime.State;

namespace WitchPantry.Tests.EditMode.Runtime.State
{
    public class MachineOwnershipStateTests
    {
        [Test]
        public void AddMachine_NewMachine_Succeeds()
        {
            var state = new MachineOwnershipState();

            var added = state.AddMachine("machine.cauldron", 1, 1, true);

            Assert.That(added, Is.True);
            Assert.That(state.OwnedMachines["machine.cauldron"].upgradeLevel, Is.EqualTo(1));
        }

        [Test]
        public void AddMachine_DuplicateMachine_Fails()
        {
            var state = new MachineOwnershipState();
            state.AddMachine("machine.cauldron", 1, 1, true);

            var added = state.AddMachine("machine.cauldron", 2, 3, true);

            Assert.That(added, Is.False);
            Assert.That(state.OwnedMachines["machine.cauldron"].upgradeLevel, Is.EqualTo(1));
        }

        [Test]
        public void UpdateMachine_ExistingMachine_Succeeds()
        {
            var state = new MachineOwnershipState();
            state.AddMachine("machine.cauldron", 1, 1, true);

            var updated = state.UpdateMachine("machine.cauldron", 2, 4);

            Assert.That(updated, Is.True);
            Assert.That(state.OwnedMachines["machine.cauldron"].upgradeLevel, Is.EqualTo(2));
            Assert.That(state.OwnedMachines["machine.cauldron"].count, Is.EqualTo(4));
        }

        [Test]
        public void UpdateMachine_MissingMachine_Fails()
        {
            var state = new MachineOwnershipState();

            var updated = state.UpdateMachine("machine.cauldron", 2, 4);

            Assert.That(updated, Is.False);
            Assert.That(state.OwnedMachines, Is.Empty);
        }

        [Test]
        public void ReadOnlyCollection_ReflectsValidChanges()
        {
            var state = new MachineOwnershipState();
            var machines = state.OwnedMachines;

            state.AddMachine("machine.cauldron", 1, 1, true);

            Assert.That(machines.ContainsKey("machine.cauldron"), Is.True);
        }

        [Test]
        public void Events_FireOnlyOnRealChanges()
        {
            var state = new MachineOwnershipState();
            var machinesChangedCount = 0;
            var machineChangedCount = 0;
            string changedMachineId = null;
            state.MachinesChanged += () => machinesChangedCount++;
            state.MachineChanged += machineId =>
            {
                machineChangedCount++;
                changedMachineId = machineId;
            };

            state.UpdateMachine("machine.cauldron", 1, 1);
            state.AddMachine("machine.cauldron", 1, 1, true);
            state.AddMachine("machine.cauldron", 1, 1, true);
            state.UpdateMachine("machine.cauldron", 1, 1);
            state.UpdateMachine("machine.cauldron", 2, 1);

            Assert.That(machinesChangedCount, Is.EqualTo(2));
            Assert.That(machineChangedCount, Is.EqualTo(2));
            Assert.That(changedMachineId, Is.EqualTo("machine.cauldron"));
        }
    }
}
