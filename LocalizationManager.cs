using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageChangedEventArgs : EventArgs
{
    public SystemLanguage OldLanguage { get; set; }
    public SystemLanguage NewLanguage { get; set; }
}

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

    [SerializeField]
    private TMP_FontAsset m_English;

    [SerializeField]
    private TMP_FontAsset m_French;

    [SerializeField]
    private TMP_FontAsset m_Italian;

    [SerializeField]
    private TMP_FontAsset m_Portuguese;

    [SerializeField]
    private TMP_FontAsset m_German;

    [SerializeField]
    private TMP_FontAsset m_Spanish;

    [SerializeField]
    private TMP_FontAsset m_Japanese;

    [SerializeField]
    private TMP_FontAsset m_Chinese;

    private SystemLanguage m_currentLanguage = SystemLanguage.English;
    private SystemLanguage m_oldLanguage = SystemLanguage.English;
    private Dictionary<string, string> m_stringLookup;

    private List<SystemLanguage> m_supportedLanguages = new List<SystemLanguage>() {
        SystemLanguage.English,
        SystemLanguage.French,
        SystemLanguage.Italian,
        SystemLanguage.German,
        SystemLanguage.Spanish,
        SystemLanguage.Portuguese,
        SystemLanguage.Japanese,
        SystemLanguage.ChineseSimplified,
     };

    private void Awake()
    {
        Instance = this;
    }

    public List<SystemLanguage> GetSupportedLanguages()
    {
        return m_supportedLanguages;
    }

    public SystemLanguage GetCurrentLanguage()
    {
        return m_currentLanguage;
    }

    public TMP_FontAsset GetFont()
    {
        switch (m_currentLanguage)
        {
            case SystemLanguage.English:
                return m_English;
            case SystemLanguage.French:
                return m_French;
            case SystemLanguage.Italian:
                return m_Italian;
            case SystemLanguage.German:
                return m_German;
            case SystemLanguage.Portuguese:
                return m_Portuguese;
            case SystemLanguage.Spanish:
                return m_Spanish;
            case SystemLanguage.Japanese:
                return m_Japanese;
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
                return m_Chinese;
        }

        return m_English;
    }

    public void ChangeToNextLanguage()
    {
        int currentIndex = m_supportedLanguages.IndexOf(m_currentLanguage);
        currentIndex++;

        if (currentIndex >= m_supportedLanguages.Count)
        {
            currentIndex = 0;
        }

        ChangeLanguage(m_supportedLanguages[currentIndex]);
    }

    public void ChangeLanguage(SystemLanguage newLanguage)
    {
        m_oldLanguage = m_currentLanguage;
        m_currentLanguage = newLanguage;

        if (LoadLanguage())
        {
            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs() { OldLanguage = m_oldLanguage, NewLanguage = m_currentLanguage });
        }
        else
        {
            ChangeLanguage(SystemLanguage.English);
        }

        Player.Instance.SavePlayerData();
    }

    public LanguageChangedEventArgs GetLanguageChangedEventArgs()
    {
        return new LanguageChangedEventArgs() { OldLanguage = m_oldLanguage, NewLanguage = m_currentLanguage };
    }

    public string GetString(string stringID)
    {
        if (m_stringLookup != null && m_stringLookup.TryGetValue(stringID, out string value))
        {
            return value;
        }
        else
        {
            return stringID;
        }
    }

    private bool LoadLanguage()
    {
        try
        {
            string languagePath = string.Format("Strings/{0}", m_currentLanguage);
            TextAsset languageCSV = (TextAsset)Resources.Load(languagePath, typeof(TextAsset));
            m_stringLookup = new Dictionary<string, string>();

            string[] entries = languageCSV.text.Split("\r\n");

            foreach (string entry in entries)
            {
                var split = entry.Split(",", 2);

                var key = split[0];
                var value = split[1];

                if (key == string.Empty || value == string.Empty)
                {
                    continue;
                }

                m_stringLookup.Add(split[0], split[1]);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            ChangeToNextLanguage();
        }
    }
}
