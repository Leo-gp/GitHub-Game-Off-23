using System;
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
        ///     The current fish entity used for damage
        /// </summary>
        private readonly Fish fish;

        /// <summary>
        ///     Triggered once a fish has been completely scaled.
        /// </summary>
        public readonly UnityEvent OnFishHasBeenScaled = new();

        /// <summary>
        ///     Triggered when a fish is scaled
        /// </summary>
        public readonly UnityEvent<int> OnFishHasReceivedDamage = new();

        public FishService(Fish fish)
        {
            this.fish = fish;
        }

        /// <summary>
        ///     Sets the current fish to a new fish instance.
        ///     Note that this should only be done if the current fish is null or its remaining scales are zero
        /// </summary>
        private void SpawnNewFish()
        {
            Assert.IsTrue(fish is null || fish.RemainingScales is 0,
                "Should only try to spawn a new fish if there is none yet or the current fish has been scaled");

            // Change this behaviour if there will be different fish types soon
            RestoreFishScales();
            LogInfo($"Spawned a new fish with a total amount of scales of '{fish.RemainingScales}");
        }

        public int GetBaseScalesOfCurrentFish()
        {
            return fish.TotalScales;
        }

        /// <summary>
        ///     Damages the current fish by the specified amount.
        ///     If the fish "dies" / is scaled, a new fish is spawned and the carry-over damage will be applied to
        ///     that new fish.
        /// </summary>
        /// <param name="damage">the positive amount of damage to deal</param>
        public void ScaleFish(int damage)
        {
            Assert.IsFalse(damage < 0, "Damage dealt cannot be negative");
            Assert.IsFalse(fish is null || fish.RemainingScales is 0,
                "Should not try to scale the fish if it does not exist yet or is already scaled");

            var carryOverDamage = damage;

            while (carryOverDamage > 0)
            {
                fish.RemainingScales -= carryOverDamage;
                OnFishHasReceivedDamage.Invoke(carryOverDamage);
                LogInfo($"Damaged the current fish by '{carryOverDamage}'");

                LogInfo("Now checking if the fish has been scaled completely");
                if (fish.RemainingScales <= 0)
                {
                    LogInfo("The fish has been scaled");

                    LogInfo("Triggering the successful scale event");
                    OnFishHasBeenScaled.Invoke();

                    carryOverDamage = Math.Abs(fish.RemainingScales);
                    LogInfo($"There is a carry over damage of '{carryOverDamage}'");

                    // For nicer consistency in the program
                    fish.RemainingScales = 0;

                    SpawnNewFish();

                    if (carryOverDamage > 0)
                    {
                        LogInfo("Because there is carry over damage, the loop will process the next fish " +
                                $"to scale it. Carry over is {carryOverDamage}");
                    }
                }
                else
                {
                    LogInfo("Fish has not been scaled entirely, yet. It still has " +
                            $"'{fish.RemainingScales}' remaining scales");
                    carryOverDamage = 0;
                }
            }
        }

        private void RestoreFishScales()
        {
            fish.RemainingScales = fish.TotalScales;
        }
    }
}