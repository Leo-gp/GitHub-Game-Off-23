using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace main.view.Canvas
{
    public class PlayerHandCanvas : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public static PlayerHandCanvas Instance { get; private set; }

        public Camera PooledMainCamera { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance);
            Instance = this;
        }

        private void Start()
        {
            PooledMainCamera = Camera.main;
            Assert.IsNotNull(PooledMainCamera);
        }

        public void SetAsDirectChild([NotNull] Transform child)
        {
            child.SetParent(_rectTransform);
        }
    }
}