using System;
using System.Collections.Generic;
using System.Linq;
using main.service.Card_Management;
using ModestTree;

namespace main.service.Turn_System
{
    public class TurnPhaseActors
    {
        public List<ITurnDrawPhaseActor> TurnDrawActors { get; }
        public List<ITurnPlayPhaseActor> TurnPlayActors { get; }
        public List<ITurnEndPhaseActor> TurnEndActors { get; }

        public TurnPhaseActors(IList<ITurnPhaseActor> turnPhaseActors)
        {
            TurnDrawActors = GetTurnPhaseActors<ITurnDrawPhaseActor>(turnPhaseActors);
            TurnPlayActors = GetTurnPhaseActors<ITurnPlayPhaseActor>(turnPhaseActors);
            TurnEndActors = GetTurnPhaseActors<ITurnEndPhaseActor>(turnPhaseActors);
        }

        private static List<T> GetTurnPhaseActors<T>(IEnumerable<ITurnPhaseActor> turnPhaseActors) where T : ITurnPhaseActor
        {
            var turnPhaseActorsImplementationsSequence = GetTurnPhaseActorsImplementationsSequence();
    
            var actorImplementationsSequence = turnPhaseActorsImplementationsSequence.GetValueOrDefault(typeof(T), Array.Empty<Type>());

            UnityEngine.Assertions.Assert.IsTrue(actorImplementationsSequence.Length > 0, 
                $"No {typeof(ITurnPhaseActor)} implementations found for {typeof(T)} within {nameof(GetTurnPhaseActorsImplementationsSequence)}.");
            
            return turnPhaseActors
                .OfType<T>()
                .OrderBy(actor => actorImplementationsSequence.IndexOf(actor.GetType()))
                .ToList();
        }

        private static Dictionary<Type, Type[]> GetTurnPhaseActorsImplementationsSequence()
        {
            var drawPhaseActorsSequence = new[]
            {
                typeof(GameService),
                typeof(PlayerHandService)
            };

            var playPhaseActorsSequence = new[]
            {
                typeof(GameService)
            };

            var endPhaseActorsSequence = new[]
            {
                typeof(EffectAssemblyService),
                typeof(GameService),
                typeof(PlayerHandService),
                typeof(CardSwapService)
            };
            
            var turnPhaseActorsImplementationsSequence = new Dictionary<Type, Type[]>
            {
                { typeof(ITurnDrawPhaseActor), drawPhaseActorsSequence },
                { typeof(ITurnPlayPhaseActor), playPhaseActorsSequence },
                { typeof(ITurnEndPhaseActor), endPhaseActorsSequence }
            };

            ValidateSequencesContainAllActorsImplementations();

            return turnPhaseActorsImplementationsSequence;

            void ValidateSequencesContainAllActorsImplementations()
            {
                foreach (var (interfaceType, implementationsSequence) in turnPhaseActorsImplementationsSequence)
                {
                    var interfaceImplementations = interfaceType
                        .Assembly
                        .GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t))
                        .ToArray();

                    UnityEngine.Assertions.Assert.IsTrue(interfaceImplementations.All(implementationsSequence.Contains),
                        $"Missing implementations for interface {interfaceType.FullName}");
                }
            }
        }
    }
}