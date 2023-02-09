using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizableTextMesh : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_text;

    [SerializeField]
    private string m_stringID;

    private List<string> m_formatterArgs;

    private void Start()
    {
        m_text.font = LocalizationManager.Instance.GetFont();
        LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        UpdateLanguage(LocalizationManager.Instance.GetLanguageChangedEventArgs());
    }

    private void OnEnable()
    {
        if (LocalizationManager.Instance != null)
        {
            UpdateLanguage(LocalizationManager.Instance.GetLanguageChangedEventArgs());
        }
    }

    private void OnDestroy()
    {
        LocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
    }

    public void OnLanguageChanged(object sender, LanguageChangedEventArgs eventArgs)
    {
        UpdateLanguage(eventArgs);
    }

    private void UpdateLanguage(LanguageChangedEventArgs eventArgs)
    {
        if (LocalizationManager.Instance == null)
        {
            return;
        }

        m_text.font = LocalizationManager.Instance.GetFont();

        UpdateText();
    }

    public string GetStringID()
    {
        return m_stringID;
    }

    public void SetText(string stringID, List<string> formatterArgs = null)
    {
        m_stringID = stringID;
        m_formatterArgs = formatterArgs;

        UpdateText();
    }

    public string GetLocalizedText()
    {
        return m_text.text;
    }

    public void SetLocalizedText(string localized)
    {
        m_stringID = string.Empty;
        m_text.text = localized;
    }

    public void SetFormatterArgs(List<string> formatterArgs)
    {
        m_formatterArgs = formatterArgs;

        UpdateText();
    }

    private void UpdateText()
    {
        if (m_stringID == string.Empty)
        {
            return;
        }

        string localized = m_stringID;

        if (LocalizationManager.Instance != null)
        {
            localized = LocalizationManager.Instance.GetString(m_stringID);
        }

        if (m_formatterArgs != null)
        {
            localized = string.Format(localized, m_formatterArgs.ToArray());
        }

        m_text.text = localized;
    }
}
