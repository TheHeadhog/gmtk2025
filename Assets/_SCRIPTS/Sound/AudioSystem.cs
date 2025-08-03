using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioSystem : MonoBehaviour
{
    public static AudioSystem Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip clickSound;
    public AudioClip backgroundMusic;
    public AudioClip chatNewMessage;
    public AudioClip errorSound;
    public AudioClip mailNew;
    public AudioClip popupOpen;
    public AudioClip systemBootUp;
    public AudioClip systemBootUpVoiceover;
    [FormerlySerializedAs("pause")] public AudioClip pauseMusic;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep across scenes
        }
        else
        {
            Destroy(gameObject);
        }
        
        
    }

    public void Play(AudioSoundType soundType)
    {
            var sourceClip =  GetClip(soundType);
        if (IsMusic(soundType))
        {
            musicSource.clip =sourceClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            sfxSource.loop = false;
            sfxSource.PlayOneShot(sourceClip);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    private AudioClip GetClip(AudioSoundType soundType)
    {
        return soundType switch
        {
            AudioSoundType.ClickSound => clickSound,
            AudioSoundType.BackgroundMusic => backgroundMusic,
            AudioSoundType.ChatNewMessage => chatNewMessage,
            AudioSoundType.ErrorSound => errorSound,
            AudioSoundType.MailNewMessage => mailNew,
            AudioSoundType.PopupOpen => popupOpen,
            AudioSoundType.SystemBootUp => systemBootUp,
            AudioSoundType.SystemBootUpVoiceover => systemBootUpVoiceover,
            AudioSoundType.PauseMusic => pauseMusic,
            _ => throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null)
        };
    }
    private static bool IsMusic(AudioSoundType soundType)
    {
        return soundType switch
        {
            AudioSoundType.BackgroundMusic => true,
            AudioSoundType.PauseMusic => true,
            _ => false
        };
    }
}

public enum AudioSoundType
{
    ClickSound,
    BackgroundMusic,
    ChatNewMessage,
    ErrorSound,
    MailNewMessage,
    PopupOpen,
    SystemBootUp,
    SystemBootUpVoiceover,
    PauseMusic,
}
