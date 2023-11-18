using main.infrastructure;
using UnityEngine;

namespace main.repository
{
    public class ResourceLoader<T> : IResourceLoader<T> where T : Object
    {
        public string ResourcePath { get; }

        public ResourceLoader(LocalizationSettingsWrapper localizationSettingsWrapper, ResourcePath resourcePath)
        {
            ResourcePath = $"{localizationSettingsWrapper.SelectedLocaleIdentifier}/{resourcePath.GetValue()}";
        }

        public T[] GetAll()
        {
            return Resources.LoadAll<T>(ResourcePath);
        }
    }
}