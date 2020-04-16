using System;
using UnityEngine;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Watson.TextToSpeech.V1;
using System.Collections;
using IBM.Cloud.SDK.Utilities;

namespace IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1
{
    public class TTS
    {
        public TextToSpeechService tts;
        public AudioSource audioSource;

        private string apiKey = "VnxnnSeufHYPPhjiERJ25GZ1g8WAn6S2BwjJ9JQWf5N5";
        private string url = "https://api.eu-gb.text-to-speech.watson.cloud.ibm.com/instances/39fb4761-4a5b-4215-8f3f-2cd5841a241c";
        private IamAuthenticator authenticator;
    
        public IEnumerator SynthesizeText(string textToRead,  NarratorBehaviour output)
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
            output.textToSpeechClip = clip;
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