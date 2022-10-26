using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class GameEffectsManager : MonoBehaviour
    {
        public static GameEffectsManager Instance;
        public void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }
        #region Floating Numbers
        public FloatingNumber floatingNumberPrefab;
        private List<FloatingNumber> _floatingNumbers = new List<FloatingNumber>();

        public void SpawnFloatingNumber(int number, bool crit, Vector2 position)
        {
            FloatingNumber floatingNumber = GetFloatingNumber();
            floatingNumber.gameObject.SetActive(true);
            floatingNumber.ShowNumber(number, crit ? Color.red : Color.white);
            floatingNumber.transform.position = position;
        }
        public FloatingNumber GetFloatingNumber()
        {
            for (int i = 0; i < _floatingNumbers.Count; i++)
            {
                if (!_floatingNumbers[i].gameObject.activeInHierarchy)
                {
                    return _floatingNumbers[i];
                }
            }
            FloatingNumber floatingNumber = Instantiate(floatingNumberPrefab, transform);
            _floatingNumbers.Add(floatingNumber);
            return floatingNumber;
        }
        #endregion

        #region Freeze Frame
        private Coroutine _freezeFrame;
        public void FreezeFrame(float duration, float timeScale = 0.2f)
        {
            if (_freezeFrame != null) { StopCoroutine(_freezeFrame); }
            _freezeFrame = StartCoroutine(DoFreezeFrame());

            IEnumerator DoFreezeFrame()
            {
                Time.timeScale = timeScale;
                yield return new WaitForSecondsRealtime(duration);
                Time.timeScale = 1f;
            }
        }
        #endregion
    }
}