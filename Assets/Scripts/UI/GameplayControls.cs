using UnityEngine;
using UnityEngine.UI;

public class GameplayControls : MonoBehaviour
{
    public int currTimeScale = 1;
    public Sprite musicOnImg, musicOffImg;
    public AudioSource musicSource;

    bool _isMusicOn = true;

    public void TimeScaleSet(int scale)
    {
        Time.timeScale = scale;
        currTimeScale = scale;
    }

    public void ToggleAudio(Image img)
    {
        if(_isMusicOn)
        {
            _isMusicOn = false;
            img.sprite = musicOffImg;
            musicSource.volume = 0;
        }
        else if(!_isMusicOn)
        {
            _isMusicOn = true;
            img.sprite = musicOnImg;
            musicSource.volume = 0.3f;
        }
    }
}
