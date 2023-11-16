using main.entity.Fish_Management;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Fish_Management
{
    /// <summary>
    ///     This service provides the business logic for the fish entity, which allows spawning new fish when scaled,
    ///     and scaling fish including a way to calculate carry-over-damage.
    /// </summary>
    public class FishService : Service
    {
        /// <summary>
        ///     Triggered once a fish has been completely scaled.
        /// </summary>
        public readonly UnityEvent OnFishHasBeenScaled = new();

        /// <summary>
        ///     The current fish entity used for damage
        /// </summary>
        private Fish _currentFish;

        /// <summary>
        ///     Creates the singleton instanec
        /// </summary>
        public FishService()
        {
            Instance ??= this;
            LogInfo("Successfully set the FishService's singleton instance");

            LogInfo("Now spawning the starter fish");
            SpawnNewFish();
        }

        /// <summary>
        ///     The instance of the service singleton
        /// </summary>
        public static FishService Instance { get; private set; }

        /// <summary>
        ///     Sets the current fish to a new fish instance.
        ///     Note that this should only be done if the current fish is null or its remaining scales are zero
        /// </summary>
        private void SpawnNewFish()
        {
            Assert.IsTrue(_currentFish is null || _currentFish.remainingScales is 0,
                "Should only try to spawn a new fish if there is none yet or the current fish has been scaled");

            // Change this behaviour if there will be different fish types soon
            _currentFish = new Fish(100);
            LogInfo($"Spawned a new fish with a total amount of scales of '{_currentFish.remainingScales}");
        }

        /// <summary>
        ///     Damages the current fish by the specified amount.
        ///     If the fish "dies" / is scaled, a new fish is spawned and the carry-over damage will be applied to
        ///     that new fish.
        /// </summary>
        /// <param name="damage">the positive amount of damage to deal</param>
        public void ScaleFish(int damage)
        {
            Assert.IsTrue(damage > 0, "Damage dealt should be positive");
            Assert.IsFalse(_currentFish is null || _currentFish.remainingScales is 0,
                "Should not try to scale the fish if it does not exist yet or is already scaled");

            _currentFish.remainingScales -= damage;
            LogInfo($"Damaged the current fish by '{damage}'");

            LogInfo("Now checking if the fish has been scaled completely");
            if (_currentFish.remainingScales <= 0)
            {
                LogInfo("The fish has been scaled");

                LogInfo("Triggering the successful scale event");
                OnFishHasBeenScaled.Invoke();

                var carryOverDamage = +_currentFish.remainingScales;
                LogInfo($"There is a carry over damage of '{carryOverDamage}'");

                // For nicer consistency in the program
                _currentFish.remainingScales = 0;

                SpawnNewFish();

                if (carryOverDamage <= 0) return;

                LogInfo("Because there is carry over damage, the method will call itself recursively " +
                        "to scale the next fish");
                ScaleFish(carryOverDamage);
            }
            else
            {
                LogInfo("Fish has not been scaled entirely, yet. It still has " +
                        $"'{_currentFish.remainingScales}' remaining scales");
            }
        }
    }
}