using System.Collections;
using UnityEngine;

public class BossMusicManager : MonoBehaviour
{
    [Header("Música do Cenário")]
    [SerializeField] private AudioSource cenarioSource; // Musica do background

    [Header("Músicas do Boss")]
    [SerializeField] private AudioSource introSource;
    [SerializeField] private AudioSource loopSource;

    [Header("Configurações de Transição")]
    [SerializeField] private float tempoFadeOut = 1.5f; // Tempo do fadeOut

    private bool bossBattleStarted = false;
    private float volumeOriginalCenario;

 

    public void StartBossMusic()
    {
        if (bossBattleStarted) return;
        bossBattleStarted = true;

        // Diminui a do cenário e toca a do boss
        StartCoroutine(TransitionToBossMusic());
    }

    private IEnumerator TransitionToBossMusic()
    {
        // Faz o Fade-Out da música do cenário
        if (cenarioSource != null && cenarioSource.isPlaying)
        {
            float currentTime = 0;
            float startVolume = cenarioSource.volume;

            while (currentTime < tempoFadeOut)
            {
                currentTime += Time.deltaTime;
                cenarioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / tempoFadeOut);
                yield return null;
            }

            cenarioSource.Stop(); // Para de vez após o fade
        }

        // Toca a musica de loop
        introSource.Play();

        //Espera o tempo da intro terminar
        yield return new WaitForSeconds(introSource.clip.length);

        // Começa o loop da batalha
        if (bossBattleStarted)
        {
            loopSource.Play();
        }
    }


}