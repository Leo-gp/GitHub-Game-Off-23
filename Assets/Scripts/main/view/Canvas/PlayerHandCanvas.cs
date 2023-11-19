using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace main.view.Canvas
{
    public class PlayerHandCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public static PlayerHandCanvas Instance { get; private set; }

        private void Start()
        {
            Assert.IsNull(Instance);
            Instance = this;
        }

        public void SetAsDirectChild([NotNull] Transform child)
        {
            child.SetParent(_rectTransform);
        }
    }
}