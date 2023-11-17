using main.entity.Card_Management.Deck_Definition;
using main.repository;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace test.EditMode.integration.main.repository
{
    [TestFixture]
    public class ResourceLoaderTest
    {
        private const string ResourcesPath = "Assets/Resources";
        private const string TestFolderName = "Test";
        
        private static string TestFolderPath => $"{ResourcesPath}/{TestFolderName}";
        
        private ResourceLoader<Object> resourceLoader;

        [SetUp]
        public void SetUp()
        {
            AssetDatabase.CreateFolder(ResourcesPath, TestFolderName);

            AssetDatabase.Refresh();
        }

        [TearDown]
        public void TearDown()
        {
            AssetDatabase.DeleteAsset(TestFolderPath);
            
            AssetDatabase.Refresh();
        }

        [TestFixture]
        public class GetAllTest : ResourceLoaderTest
        {
            [Test]
            public void WhenPathExists_returnAllAssets()
            {
                var asset1 = ScriptableObject.CreateInstance<StarterDeckDefinition>();
                var asset2 = ScriptableObject.CreateInstance<StarterDeckDefinition>();
                
                AssetDatabase.CreateAsset(asset1, $"{TestFolderPath}/asset1.asset");
                AssetDatabase.CreateAsset(asset2, $"{TestFolderPath}/asset2.asset");
                
                AssetDatabase.SaveAssets();
                
                resourceLoader = new ResourceLoader<Object>(TestFolderName);
                
                var loadedObjects = resourceLoader.GetAll();

                Assert.IsNotNull(loadedObjects);
                
                Assert.That(loadedObjects, Has.Length.EqualTo(2));
                
                Assert.That(loadedObjects, Is.EquivalentTo(new[] { asset1, asset2 }));
            }

            [Test]
            public void WhenPathDoesNotExist_returnEmpty()
            {
                resourceLoader = new ResourceLoader<Object>("IncorrectPath");
                
                var loadedObjects = resourceLoader.GetAll();

                Assert.IsNotNull(loadedObjects);
                
                Assert.IsEmpty(loadedObjects);
            }
        }
    }
}