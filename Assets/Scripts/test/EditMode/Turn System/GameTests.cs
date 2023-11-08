using System.Collections;
using main.service.Turn_System;
using UnityEngine.TestTools;

namespace test.EditMode.Turn_System
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator Sample()
        {
            // Create a new game
            new GameService();

            yield return null;
        }
    }
}