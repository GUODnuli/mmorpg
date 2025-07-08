using UnityEngine;
using Managers;

class Config
{
    public static bool MusicOn
    {
        get
        {
            return PlayerPrefs.GetInt("Music", 1) == 1;
        }

        set
        {
            PlayerPrefs.SetInt("Music", value ? 1 : 0);
            SoundManager.Instance.musicOn = value;
        }
    }

    public static bool SoundOn
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }

        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
            SoundManager.Instance.soundOn = value;
        }
    }

    public static int MusicVolume
    {
        get
        {
            return PlayerPrefs.GetInt("MusicVolume", 100);
        }

        set
        {
            PlayerPrefs.SetInt("MusicVolume", value);
            SoundManager.Instance.musicVolume = value;
        }
    }

    public static int SoundVolume
    {
        get
        {
            return PlayerPrefs.GetInt("SoundVolume", 100);
        }

        set
        {
            PlayerPrefs.SetInt("SoundVolume", value);
            SoundManager.Instance.soundVolume = value;
        }
    }

    ~Config()
    {
        PlayerPrefs.Save();
    }
}