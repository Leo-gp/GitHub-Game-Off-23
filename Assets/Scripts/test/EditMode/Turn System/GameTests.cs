using System.Collections;
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

            yield return null;
        }
    }
}