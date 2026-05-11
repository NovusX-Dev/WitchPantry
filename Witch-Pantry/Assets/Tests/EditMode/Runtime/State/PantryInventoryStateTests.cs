using NUnit.Framework;
using WitchPantry.Runtime.State;

namespace WitchPantry.Tests.EditMode.Runtime.State
{
    public class PantryInventoryStateTests
    {
        [Test]
        public void AddResource_PositiveAmount_UpdatesInventory()
        {
            var state = new PantryInventoryState();

            state.AddResource("ingredient.herb", 3);
            state.AddResource("ingredient.herb", 2);

            Assert.That(state.GetResourceAmount("ingredient.herb"), Is.EqualTo(5));
            Assert.That(state.Resources["ingredient.herb"], Is.EqualTo(5));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void AddResource_ZeroOrNegativeAmount_DoesNothing(int amount)
        {
            var state = new PantryInventoryState();

            state.AddResource("ingredient.herb", amount);

            Assert.That(state.Resources, Is.Empty);
        }

        [Test]
        public void TryConsume_ExactAmount_Succeeds()
        {
            var state = new PantryInventoryState();
            state.AddResource("ingredient.herb", 3);

            var consumed = state.TryConsume("ingredient.herb", 3);

            Assert.That(consumed, Is.True);
            Assert.That(state.GetResourceAmount("ingredient.herb"), Is.EqualTo(0));
        }

        [Test]
        public void TryConsume_MoreThanAvailable_Fails()
        {
            var state = new PantryInventoryState();
            state.AddResource("ingredient.herb", 3);

            var consumed = state.TryConsume("ingredient.herb", 4);

            Assert.That(consumed, Is.False);
            Assert.That(state.GetResourceAmount("ingredient.herb"), Is.EqualTo(3));
        }

        [Test]
        public void TryConsume_MissingResource_Fails()
        {
            var state = new PantryInventoryState();

            Assert.That(state.TryConsume("ingredient.herb", 1), Is.False);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TryConsume_ZeroOrNegativeAmount_Fails(int amount)
        {
            var state = new PantryInventoryState();
            state.AddResource("ingredient.herb", 3);

            var consumed = state.TryConsume("ingredient.herb", amount);

            Assert.That(consumed, Is.False);
            Assert.That(state.GetResourceAmount("ingredient.herb"), Is.EqualTo(3));
        }

        [Test]
        public void Events_FireOnlyOnRealChanges()
        {
            var state = new PantryInventoryState();
            var inventoryChangedCount = 0;
            var resourceChangedCount = 0;
            string changedResource = null;
            var changedAmount = -1;
            state.InventoryChanged += () => inventoryChangedCount++;
            state.ResourceChanged += (resourceId, amount) =>
            {
                resourceChangedCount++;
                changedResource = resourceId;
                changedAmount = amount;
            };

            state.AddResource("ingredient.herb", 0);
            state.TryConsume("ingredient.herb", 1);
            state.AddResource("ingredient.herb", 2);
            state.TryConsume("ingredient.herb", 1);

            Assert.That(inventoryChangedCount, Is.EqualTo(2));
            Assert.That(resourceChangedCount, Is.EqualTo(2));
            Assert.That(changedResource, Is.EqualTo("ingredient.herb"));
            Assert.That(changedAmount, Is.EqualTo(1));
        }
    }
}
