using CBGames.Core;
using CBGames.UI;
using System.Collections;
using UnityEngine;

public class SetWorldMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] randomMusic = new AudioClip[] { };
    [SerializeField] private float volume = 0.5f;
    private UICoreLogic logic;

    public void PlayWorldMusic()
    {
        StartCoroutine(WaitForLogic());
    }
    IEnumerator WaitForLogic()
    {
        if (randomMusic.Length < 1) yield return null;
        yield return new WaitUntil(() => NetworkManager.networkManager != null);
        yield return new WaitUntil(() => NetworkManager.networkManager.GetComponentInChildren<UICoreLogic>());
        logic = NetworkManager.networkManager.GetComponentInChildren<UICoreLogic>();
        Debug.Log(randomMusic[Random.Range(0, randomMusic.Length)]);
        logic.SetMusicAudio(randomMusic[Random.Range(0, randomMusic.Length)]);
        logic.SetMusicVolume(0);
        logic.SetFadeToVolume(volume);
        logic.FadeMusic(false);
        logic.PlayMusic();
    }
}
