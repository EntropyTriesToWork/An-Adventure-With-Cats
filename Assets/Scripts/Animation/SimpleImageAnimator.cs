using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SimpleImageAnimator : MonoBehaviour
{
    Image _image;
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
        _image = GetComponent<Image>();
        if (PlayOnAwake)
        {
            _image.enabled = true;
            InvokeRepeating(nameof(SwitchSprites), 0, 1f / framesPerSecond);
        }
    }

    private void SwitchSprites()
    {
        if (spriteIndex == 0) { OnStartPlay.Invoke(); }
        if (sprites.Length == spriteIndex && DestroyOnEnd) { Destroy(gameObject); OnEndPlay.Invoke(); return; }
        if (sprites.Length <= spriteIndex && HideOnEnd)
        {
            _image.enabled = false;
            spriteIndex = 0;
            _image.sprite = sprites[0];
            CancelInvoke();
            OnEndPlay.Invoke();
            return;
        }
        else if (Loop && sprites.Length == spriteIndex) { spriteIndex = 0; OnEndPlay.Invoke(); }
        else
        {
            _image.sprite = sprites[spriteIndex];
            if (sprites.Length == spriteIndex)
            {
                CancelInvoke(nameof(SwitchSprites));
                _image.sprite = sprites[^1];
                return;
            }
            _image.sprite = sprites[Mathf.Clamp(spriteIndex, 0, sprites.Length)];
            spriteIndex += 1;
        }
    }
    #region Public Methods
    public void Play()
    {
        _image.enabled = true;
        InvokeRepeating(nameof(SwitchSprites), 0, 1f / framesPerSecond);
    }
    public void Pause()
    {
        CancelInvoke(nameof(SwitchSprites));
    }
    public void Restart()
    {
        spriteIndex = 0;
        _image.sprite = sprites[spriteIndex];
    }
    public void GoToEnd()
    {
        CancelInvoke(nameof(SwitchSprites));
        spriteIndex = sprites.Length - 1;
        _image.sprite = sprites[spriteIndex];
    }
    public void GoToNextImage()
    {
        if (sprites.Length == spriteIndex)
        {
            spriteIndex = 0;
            _image.sprite = sprites[spriteIndex];
        }
        else
        {
            spriteIndex++;
            _image.sprite = sprites[spriteIndex];
        }
    }
    public void SwitchToSpecificSprite(int _spriteIndex)
    {
        spriteIndex = _spriteIndex;
        _image.sprite = sprites[spriteIndex];
    }
    #endregion
}
