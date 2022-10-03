using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class FloatingNumber : MonoBehaviour
    {
        public Sprite[] zeroToNineSprites;
        private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();
        public SpriteRenderer prefabTemplate;
        public void ShowNumber(int number, Color numberColor)
        {
            prefabTemplate.color = numberColor;
            if (_sprites.Count > 0)
            {
                for (int i = 0; i < _sprites.Count; i++)
                {
                    if (_sprites[i] != null) { _sprites[i].gameObject.SetActive(false); }
                }
            }
            int[] ints = SplitIntIntoList(number);

            while (ints.Length > _sprites.Count)
            {
                _sprites.Add(Instantiate(prefabTemplate, transform));
            }
            //float totalWidth = 5 * ints.Length * 0.05f;
            for (int i = 0; i < ints.Length; i++)
            {
                _sprites[i].sprite = zeroToNineSprites[ints[i]];
                _sprites[i].gameObject.SetActive(true);
                //_sprites[i].transform.localPosition = new Vector3((i == 0 ? 0 : totalWidth / i), 0, 0);
            }
            StartCoroutine(StartCountdown());
            IEnumerator StartCountdown()
            {
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
            }
        }
        private void Update()
        {
            transform.position += Vector3.up * Time.deltaTime;
        }

        public int[] SplitIntIntoList(int value)
        {
            var numbers = new Stack<int>();

            for (; value > 0; value /= 10)
                numbers.Push(value % 10);

            return numbers.ToArray();
        }
    }
}