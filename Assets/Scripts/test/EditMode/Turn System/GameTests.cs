using System.Collections;
using main.service.Card_Management;
using main.service.Turn_System;
using UnityEngine.Localization.Settings;
using UnityEngine.TestTools;

namespace test.EditMode.Turn_System
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator Sample()
        {
            // Adds the first locale as the selected locale in CI executions 
            LocalizationSettings.SelectedLocale ??= LocalizationSettings.AvailableLocales.Locales[0];

            // Create a new game
            new GameService();

            while (GameService.Instance.GameIsRunningJustForTest)
            {
                GameService.Instance.StartTurn();
                // As a test, only the first card is always played
                PlayerHandService.Instance.PlayCardAt(0);
                GameService.Instance.EndTurn();
            }

            GameService.Instance.StartTurn();
            GameService.Instance.EndTurn();

            yield return null;
        }
    }
}