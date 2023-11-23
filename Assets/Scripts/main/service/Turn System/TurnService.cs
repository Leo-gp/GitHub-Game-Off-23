using main.entity.Turn_System;
using UnityEngine.Events;
using Zenject;

namespace main.service.Turn_System
{
    public class TurnService : Service, IInitializable
    {
        private readonly Turn turn;
        private readonly TurnPhaseActors turnPhaseActors;

        public TurnService(Turn turn, TurnPhaseActors turnPhaseActors)
        {
            this.turn = turn;
            this.turnPhaseActors = turnPhaseActors;
        }
        
        public void Initialize()
        {
            StartTurn();
        }
        
        /// <summary>
        ///     Triggered once the turn number is increased when a new turn is started.
        ///     It uses the current turn number as its argument.
        /// </summary>
        public readonly UnityEvent<int> OnTurnNumberIncreased = new();

        public void StartTurn()
        {
            IncrementTurnNumber();
            RestoreTime();

            turnPhaseActors.TurnDrawActors.ForEach(actor => actor.OnDrawStarted());
            turnPhaseActors.TurnPlayActors.ForEach(actor => actor.OnPlayPhaseStarted());
        }

        public void EndTurn()
        {
            LogInfo("Now ending the current turn");
            
            turnPhaseActors.TurnEndActors.ForEach(actor => actor.OnTurnEnded());
        }

        private void IncrementTurnNumber()
        {
            turn.CurrentTurnNumber++;
            LogInfo($"Incrementing turn number. It now is: {turn.CurrentTurnNumber}");

            OnTurnNumberIncreased.Invoke(turn.CurrentTurnNumber);
            LogInfo("Triggering the OnTurnNumberIncreased event");
        }

        private void RestoreTime()
        {
            turn.RemainingTime = turn.InitialTime;
        }
    }
}