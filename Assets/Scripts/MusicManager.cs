using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            Debug.Log("Iniciando la música.");
            audioSource.loop = true;

            // Agregar esto para asegurar que el volumen está bien
            audioSource.volume = 1.0f;
            audioSource.mute = false;

            // Esperar un momento antes de iniciar la música (por si acaso)
            Invoke("PlayMusic", 0.1f);
        }
        else
        {
            Debug.LogError("No se encontró AudioSource en MusicManager.");
        }
    }

    void PlayMusic()
    {
        audioSource.Play();
        Debug.Log("Reproduciendo música...");
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }

    
}