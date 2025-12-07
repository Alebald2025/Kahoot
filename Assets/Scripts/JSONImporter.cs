using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JSONImporter : MonoBehaviour
{
    public Button importButton;
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    private string kahootPath => Application.persistentDataPath + "/Kahoots/";

    void Start()
    {
        if (!Directory.Exists(kahootPath)) Directory.CreateDirectory(kahootPath);
        importButton.onClick.AddListener(ImportJson);
        notificationPanel.SetActive(false);
    }

    void ImportJson()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Selecciona un Kahoot JSON", "", "json");

        if (!string.IsNullOrEmpty(filePath))
        {
            string fileName = Path.GetFileName(filePath);
            string destPath = Path.Combine(kahootPath, fileName);

            File.Copy(filePath, destPath, true);

            // Refrescar lista de Kahoots
            FindObjectOfType<KahootSelector>().RefreshKahootList();

            // Mostrar notificación
            ShowNotification("JSON importado correctamente: " + fileName);
        }
        else
        {
            ShowNotification("No se seleccionó ningún archivo.");
        }
#elif UNITY_ANDROID || UNITY_IOS
        ShowNotification("⚠️ Importar JSON desde botón no está disponible en móvil.");
#endif
    }

    void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);
        Invoke("HideNotification", 3f); // Ocultar después de 3 segundos
    }

    void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}
