using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class KahootSelector : MonoBehaviour
{
    public Transform buttonContainer;
    public GameObject buttonPrefab;
    public TMP_InputField usernameInput;

    private string persistentPath => Application.persistentDataPath + "/Kahoots/";

    void Start()
    {
        LoadKahootsFromResources();
        LoadKahootsFromPersistentData();
    }

    void LoadKahootsFromPersistentData()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("KahootsDefault");
        foreach (var file in jsonFiles)
        {
            KahootData kahoot = JsonUtility.FromJson<KahootData>(file.text);
            CreateButton(kahoot.title, file.text);
        }
    }

    void LoadKahootsFromResources()
    {
        if (!Directory.Exists(persistentPath)) Directory.CreateDirectory(persistentPath);
        string[] files = Directory.GetFiles(persistentPath, "*.json");
        foreach (string path in files)
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrEmpty(json))
            {
                ErrorReporter.Report("Archivo JSON vacío o corrupto: " + path);
            }
            KahootData kahoot = JsonUtility.FromJson<KahootData>(json);
            CreateButton(kahoot.title, json);
        }
    }

    void CreateButton(string title, string jsonContent)
    {
        GameObject btn = Instantiate(buttonPrefab, buttonContainer);
        btn.GetComponentInChildren<TMP_Text>().text = title;
        btn.GetComponent<Button>().onClick.AddListener(() => OnKahootSelected(jsonContent));
    }

    void OnKahootSelected(string jsonContent)
    {
        string username = usernameInput.text;
        if (string.IsNullOrEmpty(username))
        {
            ErrorReporter.Report("Usuario no introdujo nombre en el selector de Kahoot");
            username = "#####";
        }
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("SelectedKahoot", jsonContent);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }


}
