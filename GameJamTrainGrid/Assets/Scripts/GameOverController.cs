using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartButton;
    


    private void Awake()
    {
        restartButton.onClick.AddListener(ReloadLevel);
    }
    public void PlayerDied()
    {
        SoundManager.Instance.PlayMusic(Sounds.PlayerDeath);
        gameObject.SetActive(true);
    }

    private void ReloadLevel()
    {
        SoundManager.Instance.PlayMusic(Sounds.Music);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
