using System.Collections;
using main.view.Panels;
using UnityEngine;

namespace main.view.Canvas
{
    public class FishCanvas : MonoBehaviour
    {
        private const int REQUIRED_TO_KILL = 100;
        [SerializeField] private SlashPanel _slashPrefab;

        private int _batchSize, _remainder, _amountOfSlashesToSpawn, _currentFishScales;

        private void Start()
        {
            _currentFishScales = REQUIRED_TO_KILL;
            VisualiseEndOfTurnDamage(425);
        }

        private void VisualiseEndOfTurnDamage(int damage)
        {
            if (damage is 0) return;

            _batchSize = damage / REQUIRED_TO_KILL;
            _remainder = damage % REQUIRED_TO_KILL;

            NextBatch();
        }

        private void NextBatch()
        {
            if (_batchSize is 0)
            {
                _amountOfSlashesToSpawn = TranslateDamageToSlashAmount(_remainder);
                _remainder = 0;
            }
            else
            {
                _amountOfSlashesToSpawn = TranslateDamageToSlashAmount(REQUIRED_TO_KILL);
                _batchSize--;
            }

            StartCoroutine(CreateSlash());
        }

        private static int TranslateDamageToSlashAmount(int damage)
        {
            return damage switch
            {
                >= 100 => 5,
                >= 50 => 4,
                >= 20 => 3,
                >= 10 => 2,
                _ => 1
            };
        }

        private IEnumerator CreateSlash()
        {
            Debug.Log("Slashing " + _amountOfSlashesToSpawn);
            while (_amountOfSlashesToSpawn > 0)
            {
                yield return new WaitForSeconds(0.2f);
                Instantiate(_slashPrefab, transform).Render();
                _amountOfSlashesToSpawn--;
            }

            if (_remainder is not 0) NextBatch();
        }
    }
}