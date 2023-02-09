using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateCharacterTable : MonoBehaviour
{
    [MenuItem("Localization/Get Used Characters")]
    public static void GetUsedCharacters()
    {
        List<string> paths = new List<string>() {
            "Strings/English",
            "Strings/French",
            "Strings/Italian",
            "Strings/German",
            "Strings/Spanish",
            "Strings/Portuguese",
            "Strings/Japanese",
            "Strings/ChineseSimplified"
        };

        string defaultChars = "[]0123456789×+abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        HashSet<string> characters = new HashSet<string>();

        foreach (var d in defaultChars)
        {
            characters.Add(d.ToString());
        }

        foreach (var path in paths)
        {
            TextAsset languageCSV = (TextAsset)Resources.Load(path, typeof(TextAsset));

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

                foreach (char c in value)
                {
                    string s = c.ToString().Trim();
                    if (s != string.Empty && (((int)c).ToString("X4") != "200B"))
                    {
                        characters.Add(s);
                        characters.Add(s.ToUpper());
                    }
                }
            }
        }


        Debug.LogFormat("Generated {0} characters.", characters.Count);
        Debug.Log(string.Join("", characters));
    }
}
