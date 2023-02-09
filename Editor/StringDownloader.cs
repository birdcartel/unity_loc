using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

// Scripts https://gist.github.com/bmalicoat/25d1be3ead36d33d05a5a22462d29353

[Serializable]
public class StringResponse
{
    public List<List<string>> data;
}

public class StringDownloader : MonoBehaviour
{
    private static string baseUrl = "https://script.google.com/macros/s/AKfycbw0Vt4Jd_kF2GCQO0lj3S6C0eZRKYpFEIGtHmJ7l6tBoCCfz2X7aILsuJoDbaLRrsko/exec";
    private static UnityWebRequestAsyncOperation request;

    [MenuItem("Localization/Download Strings")]
    public static void FetchAllStrings()
    {
        FetchStrings(SystemLanguage.English);
        FetchStrings(SystemLanguage.French);
        FetchStrings(SystemLanguage.Italian);
        FetchStrings(SystemLanguage.German);
        FetchStrings(SystemLanguage.Spanish);
        FetchStrings(SystemLanguage.Portuguese);
        FetchStrings(SystemLanguage.Japanese);
        FetchStrings(SystemLanguage.ChineseSimplified);

        Debug.Log("Done Fetching!");

        AssetDatabase.Refresh();
    }

    public static void FetchStrings(SystemLanguage language)
    {
        string fullUrl = string.Format(baseUrl + "?sheet={0}Export", language);

        UnityWebRequest www = UnityWebRequest.Get(fullUrl);
        request = www.SendWebRequest();

        // TODO: Make this async...and then show a progress bar on completion
        while (!request.isDone)
        {
            // Spin!
        }

        if (request.webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.webRequest.error);
        }
        else
        {
            var text = request.webRequest.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<StringResponse>(text);
            string csv = "";
            foreach (var entry in response.data)
            {
                csv += string.Format("{0},{1}\r\n", entry[0], entry[1]);
            }

            csv = csv.TrimEnd();
            string outPath = string.Format("{0}/Resources/Strings/{1}.csv", Application.dataPath, language);

            File.WriteAllText(outPath, csv);
        }
    }
}