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
    public TMP_Text scoreText;
    public Transform rankingContainer;
    public GameObject rankingEntryPrefab;
    public Button backToMenuButton;
    public Button chooseAnotherButton;

    private string username;
    private int score;
    private KahootData kahoot;
    private string resultsPath;

    [System.Serializable]
    public class ResultEntry
    {
        public string Username;
        public int Score;
    }

    [XmlRoot("Results")]
    public class ResultList
    {
        [XmlElement("Entry")]
        public List<ResultEntry> entries = new List<ResultEntry>();
    }

    void Start()
    {
        username = PlayerPrefs.GetString("Username");
        string json = PlayerPrefs.GetString("SelectedKahoot");
        kahoot = JsonUtility.FromJson<KahootData>(json);
        score = PlayerPrefs.GetInt("FinalScore");

        scoreText.text = $"Has obtenido la siguiente puntuación:\n{score}";
        resultsPath = Application.persistentDataPath + "/Results/";
        if (!Directory.Exists(resultsPath)) Directory.CreateDirectory(resultsPath);

        SaveResult();
        ShowRanking();

        SaveResult();
        ShowRanking();

        backToMenuButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });

        chooseAnotherButton.onClick.AddListener(() => {
            UnityEngine.SceneManagement.SceneManager.LoadScene("QuizzesSelector");
        });

    }
    private void SaveResult()
    {
        string filePath = resultsPath + kahoot.title + ".xml";
        ResultList resultList = LoadResults(filePath);
        resultList.entries.Add(new ResultEntry { Username = username, Score = score });

        XmlSerializer serializer = new XmlSerializer(typeof(ResultList));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, resultList);
        }
    }

    ResultList LoadResults(string path)
    {
        if (!System.IO.File.Exists(path)) return new ResultList();

        XmlSerializer serializer = new XmlSerializer(typeof(ResultList));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return (ResultList)serializer.Deserialize(stream);
        }
    }

    private void ShowRanking()
    {
        string filePath = resultsPath + kahoot.title + ".xml";
        ResultList resultList = LoadResults(filePath);

        var ordered = resultList.entries.OrderByDescending(e => e.Score).ToList();

        foreach (Transform child in rankingContainer) Destroy(child.gameObject);

        foreach (var entry in ordered)
        {
            GameObject row = Instantiate(rankingEntryPrefab, rankingContainer);

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0].text = entry.Score.ToString();      
            texts[1].text = entry.Username;              
        }
    }

}
