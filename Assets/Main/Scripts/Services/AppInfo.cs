using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OnefallGames
{
    public class AppInfo : MonoBehaviour
    {
        public static AppInfo Instance { get; private set; }

        [Header("Sharing Config")]
        public string shareText = "Can you beat my score!!!";
        public string shareSubject = "Share Via";
        public string shareUrl = "play.google.com/store/apps/details?id=com.tof.ColorsSmash";


        [Header("Homepage Url")]
        public string googlePlayStoreHomepage = "https://play.google.com/store/apps/developer?id=Onefall%20Games&hl=en";
        public string appStoreHomepage = "https://itunes.apple.com/us/developer/apple/id284417353?mt=8";

        [Header("App Url")]
        public string androidUrl = "https://play.google.com/store/apps/details?id=com.tof.ColorsSmash";
        public string iosUrl = "https://itunes.apple.com/us/app/plants-vs-zombies/id893677096?mt=8";
        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}