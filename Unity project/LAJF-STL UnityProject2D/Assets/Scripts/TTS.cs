/*using System;
using UnityEngine;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Watson.TextToSpeech.V1;
using System.Collections;
using IBM.Cloud.SDK.Utilities;

namespace IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1
{
    public class TTS : MonoBehaviour
    {
        public TextToSpeechService tts;

        private string apiKey = "VnxnnSeufHYPPhjiERJ25GZ1g8WAn6S2BwjJ9JQWf5N5";
        private string url = "https://api.eu-gb.text-to-speech.watson.cloud.ibm.com/instances/39fb4761-4a5b-4215-8f3f-2cd5841a241c";
        private IamAuthenticator authenticator;

        void Start()
        {
            authenticator = new IamAuthenticator(
                 apikey: apiKey
            );

            StartCoroutine(InitalizeService());
           
            
         


        }

        public IEnumerator readText(string textToRead)
        {

            Debug.Log("2");
            Debug.Log(tts);
            byte[] synthesizeResponse = null;
            AudioClip clip = null;
            if(tts != null)
            {
            tts.Synthesize(
                callback: (DetailedResponse<byte[]> response, IBMError error) =>
                {
                    synthesizeResponse = response.Result;
                    clip = WaveFile.ParseWAV("hello_world.wav", synthesizeResponse);
                    Debug.Log("clip");
                    Debug.Log(clip);
                },
                text: textToRead,
                voice: "en-US_AllisonVoice",
                accept: "audio/wav"
            );
            }

            while (synthesizeResponse == null)
            {
                yield return null;
            }
        }

        IEnumerator InitalizeService()
        {
            while (!authenticator.CanAuthenticate())
            {
                Debug.Log("1");
                yield return null;
            }
            var tts = new TextToSpeechService(authenticator);
            tts.SetServiceUrl(url);
            StartCoroutine(readText("hello sexy man"));      
        }

    }
}
*/