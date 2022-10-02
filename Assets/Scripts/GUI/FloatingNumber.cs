using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTimeRogue
{
    public class FloatingNumber : MonoBehaviour
    {
        public Sprite[] zeroToNineSprites;

        [Button]
        public void ShowNumber(int number)
        {
            int[] ints = SplitIntIntoList(number);
            for (int i = 0; i < ints.Length; i++)
            {
                Debug.Log(ints[i]);
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