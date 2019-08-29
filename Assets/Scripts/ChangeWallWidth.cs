using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;


static class RandomExtensions
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}

public class ChangeWallWidth : MonoBehaviour
{
    float shoulderWidth = .46f;
    KeywordRecognizer m_Recognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        var soundManager = GameObject.Find("Audio Source");
        int rounds = 1;
        float last = 0;
        float[] factorlist = { 0.35f, 0.5f, 0.65f, 0.8f, 0.95f, 1.0f, 1.1f, 1.15f, 1.35f, 1.5f, 1.65f, 1.8f, 1.95f };
        List<float> nums = new List<float>();
        for (int j = 0; j<3; j++)
        {
            new System.Random().Shuffle(factorlist);
            if (factorlist[0] == last)
            {
                factorlist[0] = factorlist[1];
                factorlist[1] = last;
            }
            
            for (int i = 0; i < 13; i++)
            {
                nums.Add(factorlist[i]);
            }
            last = factorlist[12];
        }

        //Renderer rend1 = GetComponent<Renderer>();
        transform.position = new Vector3(7.5f + shoulderWidth * nums[0], transform.position.y, transform.position.z);
        Debug.Log("Round " + rounds + " factor:"+ nums[0]+" , X: "+transform.position.x+ "\n");


        HoloToolkit.Unity.TextToSpeech t2s = soundManager.GetComponent<HoloToolkit.Unity.TextToSpeech>();
        t2s.Voice = HoloToolkit.Unity.TextToSpeechVoice.Zira;
        t2s.StartSpeaking("round one. Do you think you can pass?");
        keywords.Add("next", () =>
        {
            rounds++;

            
            t2s.Voice = HoloToolkit.Unity.TextToSpeechVoice.Zira;
            if (rounds<=39) {
                transform.position = new Vector3(7.5f+shoulderWidth*nums[rounds-1], transform.position.y, transform.position.z);
                Debug.Log("Round " + rounds + " factor:" + nums[rounds-1] + " , X: " + transform.position.x + "\n");
                t2s.StartSpeaking("round " + rounds + " do you think you can pass?");
               

            }
            else
            {
                t2s.StartSpeaking("Trial complete");
            }
            


        });

        keywords.Add("restart", () =>
        {
            //Renderer rend = GetComponent<Renderer>();

            float myX = gameObject.transform.localScale.x;
            transform.position = new Vector3(7.5f + shoulderWidth * nums[0], transform.position.y, transform.position.z);
            Debug.Log("Round " + rounds + " factor:" + nums[0] + " , X: " + transform.position.x + "\n");
            t2s.StartSpeaking("round one. Do you think you can pass?");
            rounds = 1;

        });

        keywords.Add("yes", () =>
        {
            Debug.Log("Round " + rounds +": Yes\n");

        });

        keywords.Add("no", () =>
        {
            Debug.Log("Round " + rounds + ": No\n");

        });

        m_Recognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        m_Recognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        m_Recognizer.Start();




    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
