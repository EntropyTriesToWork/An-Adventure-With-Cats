using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class Coin : MonoBehaviour
    {
        public Transform collectedEffect;
        public int coinValue = 1;

        private void Awake()
        {
            GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * 10, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.Instance?.CollectCoin(coinValue);
            collectedEffect.SetParent(null);
            collectedEffect.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}