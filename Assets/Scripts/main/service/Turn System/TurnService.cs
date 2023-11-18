using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Card_Management;

namespace main.service.Turn_System
{
    public class TurnService
    {
        private readonly Turn turn;
        private readonly PlayerHandService playerHandService;

        public TurnService(Turn turn, PlayerHandService playerHandService)
        {
            this.turn = turn;
            this.playerHandService = playerHandService;
            StartTurn();
        }

        public void StartTurn()
        {
            IncrementTurnNumber();
            RestoreTime();
            playerHandService.OnTurnStarted();
        }

        private void IncrementTurnNumber()
        {
            turn.CurrentTurnNumber++;
        }

        private void RestoreTime()
        {
            turn.RemainingTime = turn.InitialTime;
        }

        public void AddTime(UnitTime unitTime)
        {
            turn.RemainingTime.Time += unitTime.Time;
        }
        
        public void SubtractTime(UnitTime unitTime)
        {
            turn.RemainingTime.Time -= unitTime.Time;
        }
    }
}