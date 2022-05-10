using Google.Cloud.Translation.V2;
using System;
using UnityEngine;

public class GoogleTranslate : MonoBehaviour
{
    private const string Key = "AIzaSyB8HhV-DG4BgfGNFqm4zbeaBICOh9qT8S8";
    TranslationClient client;

    private void Awake()
    {
        client = TranslationClient.CreateFromApiKey(Key);
    }

    private void doTest()
    {
        string testString = "I am a string that needs to be translated to Dutch";
        TranslationResult result = client.TranslateText(testString, LanguageCodes.Dutch, LanguageCodes.English);
        Debug.Log($"Test Result: {result.TranslatedText} | (Original: {testString}");
    }

    public string TranslateText(string sourceLanguage, string targetLanguage, string sourceText)
    {
        TranslationResult result = client.TranslateText(sourceText, targetLanguage, sourceLanguage);
        Debug.Log($"Result: {result.TranslatedText}");

        return result.TranslatedText;
    }
}