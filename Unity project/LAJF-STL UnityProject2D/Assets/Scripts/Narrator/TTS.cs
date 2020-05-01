using System;
using UnityEngine;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Watson.TextToSpeech.V1;
using System.Collections;
using IBM.Cloud.SDK.Utilities;

namespace IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1
{
    public class TTS : MonoBehaviour
    {   [SerializeField]
        public TextToSpeechService tts;
        public AudioSource audioSource;

        [SerializeField]
        
        public bool isLive = false;

        private string apiKey = "xdSCHOL3tNL_40-kKkfJCJdquqp359vYP2nyHDT4E-38";
        private string url = "https://api.eu-de.text-to-speech.watson.cloud.ibm.com/instances/4cb486f1-6105-425f-a322-de1aa187f142";
        private IamAuthenticator authenticator;


        public static TTS instance;

        void Awake()
        {
            /* Singleton pattern*/
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator SynthesizeText(string textToRead,  NarratorBehaviour output)
        {
            if (textToRead == null || textToRead == "")
            {
                textToRead = "Cagin Nicolas Cage in a cage. Yep cock. Also this is a default message.";
            }
            byte[] synthesizeResponse = null;
            AudioClip clip = null;

            if (tts != null && isLive)
            {
                tts.Synthesize(
                    callback: (DetailedResponse<byte[]> response, IBMError error) =>
                    {
                        synthesizeResponse = response.Result;
                        clip = WaveFile.ParseWAV("Narrator_text.wav", synthesizeResponse);
                        Debug.Log("clip");
                        Debug.Log(clip);
                        if (error != null)
                        {
                            Debug.Log(error.ErrorMessage);
                        }

                    },
                    text: textToRead,
                    voice: "en-US_MichaelVoice",
                    accept: "audio/wav"
                );

                while (synthesizeResponse == null)
                {
                    yield return null;
                }
                output.textToSpeechClip = clip;
            }
            else
            {
                Debug.LogWarning("Custom warning message: Narrator should have spoken, but TTS is null or just in developer mode to debug mock instead");
                Debug.LogWarning("Narrator is reading this: " + textToRead);
                yield return null;
            }

        }

        public IEnumerator SynthesizeText(string textToRead, AudioList output)
        {
            if (textToRead == null || textToRead == "")
            {
                textToRead = "Cagin Nicolas Cage in a cage. Yep cock. Also this is a default message.";
            }
            byte[] synthesizeResponse = null;
            AudioClip clip = null;
            tts.Synthesize(
                callback: (DetailedResponse<byte[]> response, IBMError error) =>
                {
                    synthesizeResponse = response.Result;
                    clip = WaveFile.ParseWAV("Narrator_text.wav", synthesizeResponse);
                    Debug.Log("clip");
                    Debug.Log(clip);
                    if (error != null)
                    {
                        Debug.Log(error.ErrorMessage);
                    }

                },
                text: textToRead,
                voice: "en-US_MichaelVoice",
                accept: "audio/wav"
            );

            while (synthesizeResponse == null)
            {
                yield return null;
            }
            output.textToSpeechSource.clip = clip;
        }



        public IEnumerator InitalizeService()
        {
            authenticator = new IamAuthenticator(
                 apikey: apiKey
            );
            while (!authenticator.CanAuthenticate())
            {
                yield return null;
            }
            tts = new TextToSpeechService(authenticator);
            tts.SetServiceUrl(url);
            Debug.Log("TTS is initialized");
            //StartCoroutine(ReadText("The Hero was frozen in his track for a moment, but then he decided that he should kill the opponent"));
        }

    }
}