using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleSpriteAnimator))]
public class SpriteAnimationController : MonoBehaviour
{
    private SimpleSpriteAnimator _spriteAnimator;
    private void Awake()
    {
        _spriteAnimator = GetComponent<SimpleSpriteAnimator>();
    }


}