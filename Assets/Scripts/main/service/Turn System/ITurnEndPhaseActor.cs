namespace main.service.Turn_System
{
    public interface ITurnEndPhaseActor : ITurnPhaseActor
    {
        void OnTurnEnded();
    }
}