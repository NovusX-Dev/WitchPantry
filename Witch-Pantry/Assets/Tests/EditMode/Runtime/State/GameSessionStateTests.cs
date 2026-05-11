using NUnit.Framework;
using WitchPantry.Runtime.State;

namespace WitchPantry.Tests.EditMode.Runtime.State
{
    public class GameSessionStateTests
    {
        [Test]
        public void GoldMutationMethods_BehaveCorrectly()
        {
            var state = new GameSessionState();

            state.SetGold(10);
            state.AddGold(5);
            var spent = state.TrySpendGold(12);
            var overspent = state.TrySpendGold(99);
            state.AddGold(0);
            state.SetGold(-10);

            Assert.That(spent, Is.True);
            Assert.That(overspent, Is.False);
            Assert.That(state.Gold, Is.EqualTo(0));
        }

        [Test]
        public void ActiveRoomMutation_BehavesCorrectly()
        {
            var state = new GameSessionState();

            state.SetActiveRoom("room.main_pantry");
            state.SetActiveRoom("room.main_pantry");

            Assert.That(state.ActiveRoomId, Is.EqualTo("room.main_pantry"));
        }

        [Test]
        public void ContractAddRemoveMethods_BehaveCorrectly()
        {
            var state = new GameSessionState();
            var contract = CreateActiveContract();

            var added = state.TryAddContract(contract);
            var duplicateAdded = state.TryAddContract(contract);
            var removed = state.RemoveContract(contract);
            var removedAgain = state.RemoveContract(contract);

            Assert.That(added, Is.True);
            Assert.That(duplicateAdded, Is.False);
            Assert.That(removed, Is.True);
            Assert.That(removedAgain, Is.False);
            Assert.That(state.ActiveContracts, Is.Empty);
        }

        [Test]
        public void TryAddContract_EnforcesMaxActiveContractCount()
        {
            var state = new GameSessionState();

            Assert.That(state.TryAddContract(CreateActiveContract("contract.1")), Is.True);
            Assert.That(state.TryAddContract(CreateActiveContract("contract.2")), Is.True);
            Assert.That(state.TryAddContract(CreateActiveContract("contract.3")), Is.True);
            Assert.That(state.TryAddContract(CreateActiveContract("contract.4")), Is.False);
            Assert.That(state.ActiveContracts.Count, Is.EqualTo(GameSessionState.MaxActiveContracts));
        }

        [Test]
        public void Events_FireOnlyOnRealChanges()
        {
            var state = new GameSessionState();
            var goldChangedCount = 0;
            var activeRoomChangedCount = 0;
            var contractsChangedCount = 0;
            state.GoldChanged += _ => goldChangedCount++;
            state.ActiveRoomChanged += _ => activeRoomChangedCount++;
            state.ContractsChanged += () => contractsChangedCount++;

            state.SetGold(0);
            state.SetGold(5);
            state.TrySpendGold(99);
            state.SetActiveRoom("room.main_pantry");
            state.SetActiveRoom("room.main_pantry");

            var contract = CreateActiveContract();
            state.TryAddContract(contract);
            state.TryAddContract(contract);
            contract.UpdateContractProgress(1);
            state.RemoveContract(contract);
            contract.UpdateContractProgress(2);

            Assert.That(goldChangedCount, Is.EqualTo(1));
            Assert.That(activeRoomChangedCount, Is.EqualTo(1));
            Assert.That(contractsChangedCount, Is.EqualTo(3));
        }

        private static ContractRuntimeState CreateActiveContract(string contractId = "contract.test")
        {
            var state = new ContractRuntimeState();
            state.SetNewContract(RuntimeStateTestFactory.CreateContract(contractId));
            return state;
        }
    }
}
