using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;

namespace main.repository
{
    /// <summary>
    ///     Used to load the card pool by using the asset at the desired localized resource path
    /// </summary>
    public class CardVaultRepository : IAssetRepository<Card>
    {
        /// <summary>
        ///     Loads all card scriptable objects from the resource folder
        /// </summary>
        /// <returns>All cards of the game as an array</returns>
        public Card[] GetAll()
        {
            Assert.IsFalse(LocalizationSettings.SelectedLocale is null, "There is no locale selected");
            return Resources.LoadAll<Card>(LocalizationSettings.SelectedLocale.Identifier + "/Cards");
        }
    }
}