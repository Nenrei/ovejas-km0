using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterButton : MonoBehaviour
{
    
    string gameUrl = "https://nenrei.itch.io/ovejaskm0";
    //[SerializeField] string tweetLang = "es";
    string twitterUrl = "http://twitter.com/intent/tweet";
    string tweetText = "He hecho un tiempo";

    string hashtags = "#OvejasKm0 #indieDev #gameDev #Unity #GameJam #Trashumancia #GameJamMadridCrea";

    public string TweetText { get => tweetText; set => tweetText = value; }

    public void PublicTweet()
    {
        Application.OpenURL(twitterUrl + "?text=" + WWW.EscapeURL(TweetText + "\n" + gameUrl + "\n" + "\n" + hashtags));
    }
}
