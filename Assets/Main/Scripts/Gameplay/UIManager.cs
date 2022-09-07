using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using OnefallGames;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { private set; get; }

	#region PRIVATE_VARIABLES

	string leaderBoardID = "com.pockapp.blasthitleaderboard";

	#endregion

    //Gameplay UI
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject stateUI;
    [SerializeField] private Text stateTxt;

//	[SerializeField] private GameObject scoreUI;
	[SerializeField] private Text scoreTxt;
//	[SerializeField] private Text stageTxt;
	public Text healthTxt;
    [SerializeField] private GameObject coinUI;
    [SerializeField] private Text coinsTxt;
    [SerializeField] private GameObject bossFightUI;
    [SerializeField] private Text bossNameTxt;
    [SerializeField] private Transform knifeCountUI;
    [SerializeField] private Image gameplayPanel;

    //Revive UI
    [SerializeField] private GameObject reviveUI;
    [SerializeField] private Image reviveCoverImg;

    //GameOver UI
    [SerializeField] private GameObject gameOverUI;
//	[SerializeField] private GameObject gameMenuUI;
    [SerializeField] private Text gameNameTxt;
    [SerializeField] private Text highestStateTxt;
//    [SerializeField] private RawImage shareImage;
    [SerializeField] private GameObject freeCoinsBtn;
    [SerializeField] private Button dailyRewardBtn;
    [SerializeField] private Text dailyRewardText;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject restartBtn;
//    [SerializeField] private GameObject shareBtn;
//    [SerializeField] private GameObject soundOnBtn;
//    [SerializeField] private GameObject soundOffBtn;
//    [SerializeField] private GameObject musicOnBtn;
//    [SerializeField] private GameObject musicOffBtn;

    //References
    [SerializeField] private AnimationClip servicesBtns_Show;
    [SerializeField] private AnimationClip servicesBtns_Hide;
    [SerializeField] private AnimationClip settingBtns_Hide;
    [SerializeField] private AnimationClip settingBtns_Show;
    [SerializeField] private Animator settingAnim;
    [SerializeField] private Animator servicesAnim;


    public bool IsWatchAdToContinue { private set; get; }

    private void OnEnable()
    {
        GameManager.GameStateChanged += GameManager_GameStateChanged;
    }


    private void OnDisable()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
    }


    private void GameManager_GameStateChanged(GameState obj)
    {
		if (obj == GameState.GameOver)
        {
            StartCoroutine(ShowGameOverUI(0.5f));
        }
		else if (obj == GameState.Playing)
        {
//			gameMenuUI.SetActive (false);
            gameOverUI.SetActive(false);
            reviveUI.SetActive(false);           
            gameplayUI.SetActive(true);

            if (GameManager.Instance.IsBossState)
            {
//                stateUI.SetActive(false);
                coinUI.SetActive(false);
                bossFightUI.SetActive(true);
            }
            else
            {
                bossFightUI.SetActive(false);
                coinUI.SetActive(false);
                stateUI.SetActive(true);
                stateTxt.text = "STAGE " + StateManager.Instance.CurrentState.ToString();

            }

            StartCoroutine(FadingOutGameplayPanel());
        }
        else if (obj == GameState.Revive)
        {
            StartCoroutine(ShowReviveUI(0.5f));
        }
		else if (obj == GameState.FinishState)
        {
            StartCoroutine(FadingInGameplayPanel(0.7f));
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }




    // Use this for initialization
    void Start () {
		GameAnalytics.Initialize ();
//		StateManager.Instance.ResetState();
        reviveUI.SetActive(false);

        if (!GameManager.IsRestart) //This is the first load
        {
            gameplayUI.SetActive(false);
            gameOverUI.SetActive(true);
			highestStateTxt.text = StateManager.Instance.HighestState.ToString();
//            shareImage.gameObject.SetActive(false);
            highestStateTxt.gameObject.SetActive(true);
//			scoreTxt.text = StateManager.Instance.CurrentState.ToString ();
			scoreTxt.gameObject.SetActive(false);
//			stageTxt.gameObject.SetActive (false);
            freeCoinsBtn.SetActive(false);
            dailyRewardBtn.gameObject.SetActive(false);
            restartBtn.SetActive(false);
//            shareBtn.SetActive(false);
            playBtn.SetActive(true);
        }
        else
        {
            gameplayUI.SetActive(true);
            gameOverUI.SetActive(false);
//			gameMenuUI.SetActive (false);
        }

		LeaderboardManager.AuthenticateToGameCenter();
    }
	
	// Update is called once per frame
	void Update () {

//        UpdateMusicButtons();
//        UpdateMuteButtons();
		healthTxt.text = GameManager.Instance.healthCount.ToString();
        coinsTxt.text = CoinManager.Instance.Coins.ToString();

        if(DailyRewardManager.Instance.CanRewardNow())
        {
            dailyRewardText.text = "FREE COINS";
            dailyRewardBtn.interactable = true;
        }
        else
        {
            string hours = DailyRewardManager.Instance.TimeUntilNextReward.Hours.ToString();
            string minutes = DailyRewardManager.Instance.TimeUntilNextReward.Minutes.ToString();
            string seconds = DailyRewardManager.Instance.TimeUntilNextReward.Seconds.ToString();
            dailyRewardText.text = hours + ":" + minutes + ":" + seconds;
            dailyRewardBtn.interactable = false;
        }

	}


    ////////////////////////////Publish functions
//    public void PlayButtonSound()
//    {
//        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
//    }



    public void PlayBtn()
    {
		GameManager.scorez = 1;
        GameManager.Instance.PlayingGame();
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");

    }
    public void RestartBtn(float delay)
    {
        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name, delay);
		GameManager.scorez = 1;
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");
    }

//	public void ShareBtn()
//	{
//		ShareManager.Instance.ShareScreenshotWithText();
//	}

//    public void CharacterBtn()
//    {
//        GameManager.Instance.LoadScene("Character", 0.5f);
//    }
//    public void SettingBtn()
//    {
//        servicesAnim.Play(servicesBtns_Hide.name);
//        settingAnim.Play(settingBtns_Show.name);
//    }
//    public void ToggleSound()
//    {
//        SoundManager.Instance.ToggleMute();
//    }
//
//    public void ToggleMusic()
//    {
//        SoundManager.Instance.ToggleMusic();
//    }
	public void RateAppBtn()
	{
		Application.OpenURL("https://itunes.apple.com/us/app/blast-hit/id1407455606?ls=1&mt=8");
	}

	public void LeaderboardBtn()
	{
		LeaderboardManager.ShowLeaderboard();
	}


//    public void MoreAppsBtn()
//    {
//#if UNITY_ANDROID
//        Application.OpenURL(AppInfo.Instance.googlePlayStoreHomepage);
//#elif UNITY_IOS
//        Application.OpenURL(AppInfo.Instance.appStoreHomepage);
//#endif
//    }

//    public void BackBtn()
//    {
//        settingAnim.Play(settingBtns_Hide.name);
//        servicesAnim.Play(servicesBtns_Show.name);
//    }


    public void DailyRewardBtn()
    {
        DailyRewardManager.Instance.ResetNextRewardTime();
        GameManager.Instance.CreateCoinsForReward(DailyRewardManager.Instance.GetRandomReward(), 0.5f);
    }


    public void FreeCoinsBtn()
    {
        freeCoinsBtn.SetActive(false);
        AdManager.Instance.ShowRewardedVideoAd();
        IsWatchAdToContinue = false;
    }

    public void ReviveBtn()
    {
        reviveUI.SetActive(false);
//        AdManager.Instance.ShowRewardedVideoAd();
        IsWatchAdToContinue = true;
    }

    public void SkipBtn()
    {
        reviveUI.SetActive(false);
        GameManager.Instance.GameOver();
    }



    /////////////////////////////Private functions
//    void UpdateMuteButtons()
//    {
//        if (SoundManager.Instance.IsMuted())
//        {
//            soundOnBtn.gameObject.SetActive(false);
//            soundOffBtn.gameObject.SetActive(true);
//        }
//        else
//        {
//            soundOnBtn.gameObject.SetActive(true);
//            soundOffBtn.gameObject.SetActive(false);
//        }
//    }
//

//    void UpdateMusicButtons()
//    {
//        if (SoundManager.Instance.IsMusicOff())
//        {
//            musicOffBtn.gameObject.SetActive(true);
//            musicOnBtn.gameObject.SetActive(false);
//        }
//        else
//        {
//            musicOffBtn.gameObject.SetActive(false);
//            musicOnBtn.gameObject.SetActive(true);
//        }
//    }


    private IEnumerator ShowGameOverUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);

        gameNameTxt.gameObject.SetActive(false);
        highestStateTxt.gameObject.SetActive(true);
        highestStateTxt.text = StateManager.Instance.HighestState.ToString();

		LeaderboardManager.ReportScore(StateManager.Instance.HighestState,leaderBoardID);

		scoreTxt.text = GameManager.scorez.ToString ();
		scoreTxt.gameObject.SetActive(true);
//		stageTxt.gameObject.SetActive (true);
		int scorezGameana = GameManager.scorez;
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "game", scorezGameana);

        playBtn.SetActive(false);
        restartBtn.SetActive(true);
//        freeCoinsBtn.SetActive(AdManager.Instance.IsRewardedVideoAdReady());
        dailyRewardBtn.gameObject.SetActive(false);
//        shareBtn.SetActive(true);
//        shareImage.gameObject.SetActive(true);
//        shareImage.texture = GameManager.Instance.LoadedScrenshot();

    }

    private IEnumerator ShowReviveUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        reviveUI.SetActive(true);
        StartCoroutine(ReviveCountDown());
    }

    IEnumerator ReviveCountDown()
    {
        float t = 0;
        while (t < GameManager.Instance.reviveWaitTime)
        {
            if (!reviveUI.activeInHierarchy)
                yield break;
            t += Time.deltaTime;
            float factor = t / GameManager.Instance.reviveWaitTime;
            reviveCoverImg.fillAmount = Mathf.Lerp(1, 0, factor);
            yield return null;
        }
        reviveUI.SetActive(false);
        GameManager.Instance.GameOver();
    }

    private IEnumerator FadingOutGameplayPanel()
    {
//		if(!gameOverUI.activeSelf){
        gameplayPanel.gameObject.SetActive(true);

        float t = 0;
        float fadingTime = 1f;
        Color startColor = gameplayPanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            gameplayPanel.color = Color.Lerp(startColor, endColor, factor);
            yield return null;
        }

        gameplayPanel.gameObject.SetActive(false);
//    }
	}
    private IEnumerator FadingInGameplayPanel(float delay)
    {
//		if(!gameOverUI.activeSelf){
        yield return new WaitForSeconds(delay);

        gameplayPanel.gameObject.SetActive(true);

        float t = 0;
        float fadingTime = 1f;
        Color startColor = gameplayPanel.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            gameplayPanel.color = Color.Lerp(startColor, endColor, factor);
            yield return null;
        }

        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 0.1f);
//    }
	}


    /// <summary>
    /// Disable GameOverUI for reward coins
    /// </summary>
    public void DisableGameOverUI()
    {
        StartCoroutine(DisableAndEnableGameOverUI());
    }
    private IEnumerator DisableAndEnableGameOverUI()
    {
        gameOverUI.SetActive(false);
        yield return new WaitForSeconds(3f);
        gameOverUI.SetActive(true);

    } 

    /// <summary>
    /// Update knife images base on knifeCount
    /// </summary>
    /// <param name="knifeCount"></param>
//    public void UpdateKnifesImg(int knifeCount, Color disableColor, Color enableColor)
//    {
//        int knifeLost = knifeCountUI.childCount - knifeCount;
//        for (int i = 0; i < knifeLost; i++)
//        {
//            knifeCountUI.GetChild(i).GetComponent<Image>().color = disableColor;
//        }
//
//        for (int i = knifeLost; i < knifeCountUI.childCount; i++)
//        {
//            knifeCountUI.GetChild(i).GetComponent<Image>().color = enableColor;
//        }
//    }

    /// <summary>
    /// Set boss's name
    /// </summary>
    /// <param name="name"></param>
    public void SetBossName(string name)
    {
        bossNameTxt.text = name.ToUpper();
    }
}
