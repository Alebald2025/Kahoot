using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text scoreText;          // Texto dinámico de la puntuación
    public TMP_Text labelText;          // Texto fijo "Has obtenido la siguiente puntuación:"
    public Button leaderboardButton;
    public Button menuButton;
    public Button selectorButton;

    private int finalScore;

    void Start()
    {
        finalScore = PlayerPrefs.GetInt("FinalScore", 0);

        labelText.text = "Has obtenido la siguiente puntuación:";

        // Animación del contador
        StartCoroutine(AnimateScore(finalScore, 0.5f));

        leaderboardButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LeaderboardScene");
        });

        menuButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });

        selectorButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SelectorScene");
        });

    }

    IEnumerator AnimateScore(int targetScore, float duration)
    {
        float elapsed = 0f;
        int startScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreText.text = currentScore.ToString();
            yield return null;
        }

        scoreText.text = targetScore.ToString();
    }

}
