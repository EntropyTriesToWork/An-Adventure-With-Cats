using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class SimpleSpriteAnimator : MonoBehaviour
{
    SpriteRenderer _sr;
    [Range(1, 60)] public int framesPerSecond = 60;
    public bool DestroyOnEnd;
    public bool PlayOnAwake;
    public bool Loop;
    public bool HideOnEnd;
    public Sprite[] sprites;

    public UnityEvent OnStartPlay;
    public UnityEvent OnEndPlay;

    private int spriteIndex = 0;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        if (PlayOnAwake)
        {
            _sr.enabled = true;
            InvokeRepeating("SwitchSprites", 0, 1f / framesPerSecond);
        }
    }

    private void SwitchSprites()
    {
        if (spriteIndex == 0) { OnStartPlay.Invoke(); }
        if (sprites.Length == spriteIndex && DestroyOnEnd) { Destroy(gameObject); OnEndPlay.Invoke(); return; }
        if (sprites.Length == spriteIndex && HideOnEnd)
        {
            _sr.enabled = false;
            spriteIndex = 0;
            _sr.sprite = sprites[0];
            CancelInvoke();
            OnEndPlay.Invoke();
            return;
        }
        else if (Loop && sprites.Length == spriteIndex) { spriteIndex = 0; OnEndPlay.Invoke(); }
        else
        {
            _sr.sprite = sprites[spriteIndex];
            if (sprites.Length == spriteIndex)
            {
                CancelInvoke("SwitchSprites");
            }
            spriteIndex += 1;
        }
    }
    #region Public Methods
    public void Play()
    {
        _sr.enabled = true;
        InvokeRepeating("SwitchSprites", 0, 1f / framesPerSecond);
    }
    public void Pause()
    {
        CancelInvoke("SwitchSprites");
    }
    public void Restart()
    {
        spriteIndex = 0;
        _sr.sprite = sprites[spriteIndex];
    }
    public void GoToEnd()
    {
        CancelInvoke("SwitchSprites");
        spriteIndex = sprites.Length - 1;
        _sr.sprite = sprites[spriteIndex];
    }
    public void GoToNextImage()
    {
        if (sprites.Length == spriteIndex)
        {
            spriteIndex = 0;
            _sr.sprite = sprites[spriteIndex];
        }
        else
        {
            spriteIndex++;
            _sr.sprite = sprites[spriteIndex];
        }
    }
    public void SwitchToSpecificSprite(int _spriteIndex)
    {
        spriteIndex = _spriteIndex;
        _sr.sprite = sprites[spriteIndex];
    }
    #endregion
}
