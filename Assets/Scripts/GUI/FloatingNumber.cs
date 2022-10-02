using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class FloatingNumber : MonoBehaviour
    {
        public Sprite[] zeroToNineSprites;
        private SpriteRenderer[] _sprites = new SpriteRenderer[0];
        public SpriteRenderer prefabTemplate;
        public void ShowNumber(int number, Color numberColor)
        {
            prefabTemplate.color = numberColor;
            if (_sprites.Length > 0)
            {
                for (int i = 0; i < _sprites.Length; i++)
                {
                    if (_sprites[i] != null) { Destroy(_sprites[i].gameObject); }
                }
            }
            int[] ints = SplitIntIntoList(number);
            _sprites = new SpriteRenderer[ints.Length];
            //float totalWidth = 5 * ints.Length * 0.05f;
            for (int i = 0; i < ints.Length; i++)
            {
                _sprites[i] = Instantiate(prefabTemplate, transform);
                _sprites[i].sprite = zeroToNineSprites[ints[i]];
                _sprites[i].gameObject.SetActive(true);
                //_sprites[i].transform.localPosition = new Vector3((i == 0 ? 0 : totalWidth / i), 0, 0);
            }
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