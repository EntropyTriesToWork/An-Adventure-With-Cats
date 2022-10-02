using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace SmallTimeRogue
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        #region SessionStats
        [FoldoutGroup("Session Stats")] [SerializeField] [ReadOnly] GameState _currentGameState;
        [FoldoutGroup("Session Stats")] [SerializeField] [ReadOnly] float _gameTime;
        [FoldoutGroup("Session Stats")] [SerializeField] [ReadOnly] float _gold;
        public GameState CurrentGameState => _currentGameState;
        public float GameTime => _gameTime;
        public float Gold => _gold;
        #endregion

        #region Audio
        [FoldoutGroup("Audio")] [Required] public AudioSource sfxAudioSource;
        [FoldoutGroup("Audio")] [Required] public AudioSource guiAudioSource;
        [FoldoutGroup("Audio")] [Required] public AudioClip coinCollectionSfx;
        #endregion

        #region UI
        [FoldoutGroup("UI")] [Required] public CanvasGroup mainCanvas;
        [FoldoutGroup("UI")] [Required] public Image healthBarFill;
        [FoldoutGroup("UI")] [Required] public TMP_Text healthText;
        public void UpdateHealthBar(int currentHealth, int maxHealth)
        {
            healthBarFill.fillAmount = (float)currentHealth / (float)maxHealth;
            healthText.text = currentHealth.ToString();
        }
        #endregion

        #region Collectibles
        [FoldoutGroup("Collectible")] [Required] public GameObject coinPrefab;
        public void SpawnCoins(int amt, Vector2 position)
        {
            for (int i = 0; i < amt; i++)
            {
                Instantiate(coinPrefab, position, Quaternion.identity);
            }
        }
        public void CollectCoin(int amt)
        {
            _gold += amt;
            sfxAudioSource.PlayOneShot(coinCollectionSfx);
        }
        #endregion

        #region Audio
        public void PlayOneShotSFX(AudioClip audioClip)
        {
            sfxAudioSource.PlayOneShot(audioClip);
        }
        #endregion

        #region Messages
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _currentGameState = GameState.Normal;
            }
            else { Destroy(gameObject); }
        }
        private void Update()
        {
            if (_currentGameState == GameState.Normal)
            {
                _gameTime += Time.deltaTime;
            }
        }
        #endregion
    }
    public enum GameState
    {
        Paused,
        Normal,
        GameOver,
    }
}