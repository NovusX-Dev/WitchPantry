using NUnit.Framework;
using WitchPantry.Runtime.State;

namespace WitchPantry.Tests.EditMode.Runtime.State
{
    public class ContractRuntimeStateTests
    {
        [Test]
        public void SetNewContract_CopiesAuthoredValues()
        {
            var definition = RuntimeStateTestFactory.CreateContract("contract.healing", "potion.healing", 6, 40, 24f);
            var state = new ContractRuntimeState();

            state.SetNewContract(definition);

            Assert.That(state.CurrentContractId, Is.EqualTo("contract.healing"));
            Assert.That(state.TargetPotionId, Is.EqualTo("potion.healing"));
            Assert.That(state.AmountRequired, Is.EqualTo(6));
            Assert.That(state.RewardGold, Is.EqualTo(40));
            Assert.That(state.RemainingDuration, Is.EqualTo(24f));
            Assert.That(state.Active, Is.True);
        }

        [TestCase(-3, 0)]
        [TestCase(3, 3)]
        [TestCase(99, 5)]
        public void UpdateContractProgress_ClampsToRequiredAmount(int progress, int expected)
        {
            var state = new ContractRuntimeState();
            state.SetNewContract(RuntimeStateTestFactory.CreateContract(amountRequired: 5));

            state.UpdateContractProgress(progress);

            Assert.That(state.CurrentProgress, Is.EqualTo(expected));
        }

        [Test]
        public void CompleteContract_ResetsState()
        {
            var state = new ContractRuntimeState();
            state.SetNewContract(RuntimeStateTestFactory.CreateContract(amountRequired: 1));
            state.UpdateContractProgress(1);

            state.CompleteContract();

            Assert.That(state.Active, Is.False);
            Assert.That(state.CurrentContractId, Is.Null);
            Assert.That(state.CanBeCompleted, Is.False);
            Assert.That(state.IsExpired, Is.False);
            Assert.That(state.IsRunning, Is.False);
        }

        [Test]
        public void ExpireContract_ResetsState()
        {
            var state = new ContractRuntimeState();
            state.SetNewContract(RuntimeStateTestFactory.CreateContract());

            state.ExpireContract();

            Assert.That(state.Active, Is.False);
            Assert.That(state.CurrentContractId, Is.Null);
            Assert.That(state.CanBeCompleted, Is.False);
            Assert.That(state.IsExpired, Is.False);
            Assert.That(state.IsRunning, Is.False);
        }

        [Test]
        public void DerivedFlags_ReflectActiveContractState()
        {
            var state = new ContractRuntimeState();
            Assert.That(state.IsExpired, Is.False);
            Assert.That(state.IsRunning, Is.False);

            state.SetNewContract(RuntimeStateTestFactory.CreateContract(amountRequired: 2, durationHours: 1f));
            Assert.That(state.IsRunning, Is.True);

            state.UpdateContractProgress(2);
            Assert.That(state.CanBeCompleted, Is.True);
            Assert.That(state.IsRunning, Is.False);
        }

        [Test]
        public void UpdateContractProgress_WithSameProgress_DoesNotCompleteContract()
        {
            var state = new ContractRuntimeState();
            var completedCount = 0;
            state.ContractCompleted += () => completedCount++;
            state.SetNewContract(RuntimeStateTestFactory.CreateContract(amountRequired: 3));

            state.UpdateContractProgress(0);

            Assert.That(state.Active, Is.True);
            Assert.That(state.CurrentProgress, Is.EqualTo(0));
            Assert.That(completedCount, Is.EqualTo(0));
        }

        [Test]
        public void Events_FireOnlyOnRealChanges()
        {
            var state = new ContractRuntimeState();
            var changedCount = 0;
            var completedCount = 0;
            var expiredCount = 0;
            state.ContractChanged += () => changedCount++;
            state.ContractCompleted += () => completedCount++;
            state.ContractExpired += () => expiredCount++;

            state.UpdateContractProgress(1);
            state.CompleteContract();
            state.ExpireContract();
            state.SetNewContract(RuntimeStateTestFactory.CreateContract(amountRequired: 3));
            state.UpdateContractProgress(0);
            state.UpdateContractProgress(2);
            state.CompleteContract();
            state.ExpireContract();

            Assert.That(changedCount, Is.EqualTo(2));
            Assert.That(completedCount, Is.EqualTo(1));
            Assert.That(expiredCount, Is.EqualTo(0));
        }
    }
}
