using UnityEngine;

namespace main.repository
{
    public class ResourceLoader<T> : IResourceLoader<T> where T : Object
    {
        public string ResourcePath { get; }

        public ResourceLoader(string resourcePath)
        {
            ResourcePath = resourcePath;
        }

        public T[] GetAll()
        {
            return Resources.LoadAll<T>(ResourcePath);
        }
    }
}