using System.Collections.Generic;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;

namespace main.repository
{
    /// <summary>
    ///     Loads the starter deck definition by using the asset provided at the desired localized resource path
    /// </summary>
    public class StarterDeckDefinitionRepositoryLegacy : IAssetRepository<Card>
    {
        /// <summary>
        ///     Loads the starter deck definition scriptable object from the resource folder and adds the duplicates
        /// </summary>
        /// <returns>The definition as a list of cards including all duplicates</returns>
        public Card[] GetAll()
        {
            Assert.IsNotNull(LocalizationSettings.SelectedLocale, "There is no locale selected");

            var definition = Resources.LoadAll<StarterDeckDefinitionLegacy>
            (
                "Legacy/" + LocalizationSettings.SelectedLocale.Identifier + "/Starter Deck"
            );

            Assert.AreEqual(definition.Length, 1,
                "There should be exactly one starter deck definition in each localized folder");
            Assert.IsNotNull(definition[0], "The definition does not exist");

            var returnList = new List<Card>();
            foreach (var pair in definition[0].StarterDeck)
                for (var i = 0; i < pair.NumberOfCopies; i++)
                    returnList.Add(pair.Card);

            return returnList.ToArray();
        }
    }
}