using CardManagement;
using JetBrains.Annotations;
using main.entity.Card_Management;
using main.entity.Turn_System;
using UnityEngine.Assertions;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the <see cref="EffectAssembly" /> entity, providing a way for
    ///     card classes to add effects that should all be executed at the end of the turn. Then, once all card effects
    ///     have been executed, the list is cleared for the next turn.
    /// </summary>
    public class EffectAssemblyService
    {
        /// <summary>
        ///     The non-null <see cref="EffectAssembly" /> entity.
        ///     The instance is created automatically once the service is created.
        /// </summary>
        [NotNull] private readonly EffectAssembly _effectAssembly = new();

        /// <summary>
        ///     Creates the singleton of this service if it does not exist
        /// </summary>
        public EffectAssemblyService()
        {
            Assert.IsNull(Instance, "There already is a service singleton instance! Should not try to " +
                                    "create a new instance of a singleton!");

            Instance = this;
        }

        /// <summary>
        ///     The non-thread-safe singleton of the service
        /// </summary>
        public static EffectAssemblyService Instance { get; private set; }

        /// <summary>
        ///     Executes each end-of-turn effect assembled in the <see cref="EffectAssembly" /> and then clears the
        ///     list, making it ready for the next turn.
        /// </summary>
        public void ExecuteAll()
        {
            _effectAssembly.Effects.ForEach(effect => effect.Execute());
            Clear();
        }

        /// <summary>
        ///     Adds a non-null end-of-turn card effect to the <see cref="EffectAssembly" /> list.
        ///     Note that it will be deleted from the list at the end of the next turn!
        /// </summary>
        /// <param name="effect">The non-null <see cref="CardEffect" /> that should be executed at the end</param>
        public void AddEffect([NotNull] CardEffect effect)
        {
            _effectAssembly.Effects.Add(effect);
        }

        /// <summary>
        ///     Removes all end-of-turn effects from the <see cref="EffectAssembly" /> once they have been executed.
        /// </summary>
        private void Clear()
        {
            _effectAssembly.Effects.Clear();
        }
    }
}