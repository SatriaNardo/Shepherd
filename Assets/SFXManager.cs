using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public AudioClip buttonClickSound; // Assign this in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Create or find an AudioSource
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null)
        {
            GameObject audioSourceObject = new GameObject("ButtonSFXAudioSource");
            audioSource = audioSourceObject.AddComponent<AudioSource>();
        }

        // Find all buttons and add sound effect
        List<Button> buttons = GetComponentsInScene<Button>();
        List<Toggle> toggles = GetComponentsInScene<Toggle>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlaySound(buttonClickSound));
        }
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                PlaySound(buttonClickSound);
            });
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    private List<T> GetComponentsInScene<T>() where T : Component
    {
        List<T> results = new List<T>();
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            T[] components = rootObject.GetComponentsInChildren<T>(true); // Include inactive objects
            results.AddRange(components);
        }

        return results;
    }
}