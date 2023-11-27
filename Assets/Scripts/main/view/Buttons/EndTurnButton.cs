using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.view.Buttons
{
    public class EndTurnButton : MonoBehaviour
    {
        private Animator _animator;
        private TurnService turnService;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            turnService.OnNewTurnStart.AddListener(Lock);
        }

        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        public void OnClick()
        {
            turnService.EndTurn();
        }

        private void Lock()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EndOfTurnLock")) _animator.Play("Idle");
            _animator.Play("EndOfTurnLock");
        }
    }
}