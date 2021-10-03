using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Alert,
        Music,
        Ambient,
    }

    private static AudioManager instance;

    [SerializeField] private List<AudioSource> alertSource;
    [SerializeField] private List<AudioSource> ambientSource;
    [SerializeField] private List<AudioSource> musicSource;

    public List<AudioClip> sceneAmbient;
    public List<AudioClip> sceneMusic;

    void Start()
    {
        foreach (var clip in sceneAmbient)
        {
            Play(clip, SoundType.Ambient);
        }
        if (instance)
        {
            Destroy(gameObject);
            instance.sceneMusic = sceneMusic;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += (a, b) => OnSceneChanged();
        }
    }

    public static void Play(AudioClip clip, SoundType type = SoundType.Alert)
    {
        if (instance)
            instance.PlayClipAt(clip, Vector2.zero, type);
    }

    public static void Play(AudioClip clip, Vector2 position, SoundType type = SoundType.Alert)
    {
        if (instance)
            instance.PlayClipAt(clip, position, type);
    }

    void Update()
    {
        for (int i = 0; i < musicSource.Count; i++)
        {
            if (musicSource[i].isPlaying)
                return;
        }
        if (sceneMusic.Count > 0)
            musicSource[0].PlayOneShot(sceneMusic.Sample());
    }

    void PlayClipAt(AudioClip clip, Vector2 pos, SoundType type)
    {
        var list = type switch
        {
            SoundType.Alert => alertSource,
            SoundType.Music => ambientSource,
            SoundType.Ambient => musicSource,
            _ => alertSource,
        };
        foreach (var source in list)
        {
            if (!source.isPlaying)
            {
                source.transform.position = pos;
                if (type == SoundType.Alert)
                    source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(clip);
                return;
            }
        }
        var source2 = Instantiate(list[0], pos, Quaternion.identity, transform);
        list.Add(source2);
        source2.PlayOneShot(clip);
    }

    void OnSceneChanged()
    {
        foreach (var source in alertSource)
        {
            source.Stop();
        }
        foreach (var source in ambientSource)
        {
            source.Stop();
        }
    }
}
