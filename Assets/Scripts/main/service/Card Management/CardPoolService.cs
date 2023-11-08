using JetBrains.Annotations;
using main.entity.Card_Management;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    // TODO
    public class CardPoolService : Service
    {
        private readonly CardPool _cardPool = new();

        public void AddCard([NotNull] Card cardToAdd)
        {
            _cardPool.Pool.Add(cardToAdd);
            LogInfo($"$Added card '{cardToAdd}' to the card pool");
        }

        public void RemoveCard([NotNull] Card cardToRemove)
        {
            var refExisted = _cardPool.Pool.Remove(cardToRemove);
            Assert.IsTrue(refExisted, "Trying to remove a card from the card pool, which does not exist there");
            LogInfo($"Removed card '{cardToRemove}' from the card pool");
        }
    }
}