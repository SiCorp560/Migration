using System.Collections;
using UnityEngine;

public enum GameState { getReady, playing, oops, gameOver, roundWin };

public class GameManager : MonoBehaviour
{
    /* Used by other classes, GameManager shared info */
    // Singleton defintion
    public static GameManager S;
    // Game State
    public GameState gameState;

    // UI Elements
    public inGameMenu gameMenu;

    /*
    // UI Elements
    public TextMeshProUGUI messageOverlay, livesOverlay, scoreOverlay, countdownOverlay;
    
    /* Used internally for tracking gameplay info */
    //private readonly int LIVES_START = 3;
    //private int livesLeft;
    //private readonly int SCORE_START = 0;
    //private int score;
    //private readonly int TIMER_START = 300;
    //private int timeLeft;
    //private Coroutine levelTimer;

    // TODO: Move these to a LevelManager script
    public GameObject butterflyPrefab;

    // Current spawn location
    public Transform spawnPoint;

    /* Before anything happens, initialization of object */

    private void Awake()
    {
        // check if singleton exists already
        if (S == null)
            S = this;
        else
            Destroy(gameObject);

        // initialize game variables
        InitializeNewGame();
    }

    //void Start()
    //{
    //    DontDestroyOnLoad(this);
    //}

    /* Used to start the game/round */

    public void InitializeNewGame()
    {
        // reset game variables

        //livesLeft = LIVES_START;
        //score = SCORE_START;
        //timeLeft = TIMER_START;

        //// deactive the checkpoint, if it's been set before
        //LevelManager.S.SetCheckpoint(false);
    }

    public void StartRound()
    {
        // put the game into the getReady state
        gameState = GameState.getReady;

        // get ready coroutine
        // StartCoroutine(GetReadyState());
        PlayRound();
    }

    /* GameState: initialize gameplay */

    //public IEnumerator GetReadyState()
    //{
    //    // turn on the message
    //    messageOverlay.enabled = true;
    //    messageOverlay.text = LevelManager.S.levelName + "\nGet Ready!";

    //    // start displaying game UI
    //    livesOverlay.enabled = true;
    //    livesOverlay.text = livesLeft.ToString();

    //    scoreOverlay.enabled = true;
    //    scoreOverlay.text = score.ToString("0000");

    //    countdownOverlay.enabled = true;
    //    countdownOverlay.text = timeLeft.ToString("000");

    //    start playing background sounds
    //     SoundManager.S.StartAmbientSounds();

    //    // pause for 2 seconds
    //    yield return new WaitForSeconds(2.0f);

    //    // turn off the message
    //    messageOverlay.enabled = false;

    //    // start playing the game
    //    PlayRound();
    //}

    private void PlayRound()
    {
        // set gameState to playing
        gameState = GameState.playing;

        // start the level timer
        // levelTimer = StartCoroutine(CountdownTimer());
    }

    /* GameState: while playing */

    public void TriggerCheckpoint(Transform point, bool final)
    {
        // The player has reached a checkpoint
        spawnPoint = point;

        // play fanfare
        if (AudioManager.S != null)
            AudioManager.S.Play("MysteriousJingle");

        // If this is a checkpoint at the end of the level, trigger end screen
        if (final)
        {
            gameMenu.EndLevelLoad();
        }
    }

    //private IEnumerator CountdownTimer()
    //{
    //    while (timeLeft > 0)
    //    {
    //        // let 1 second pass
    //        yield return new WaitForSeconds(1.0f);

    //        // decrease timer by 1 second
    //        timeLeft--;

    //        // update countdown timer
    //        countdownOverlay.text = timeLeft.ToString("000");
    //    }

    //    // once timer runs out, player loses
    //    // reset the timer for the next round
    //    timeLeft = TIMER_START;

    //    // manually kill the player
    //    Destroy(LevelManager.S.playerObject);

    //    // tell GameManager that player lost
    //    PlayerLost();
    //}

    //public void PlayerCollectedCoin()
    //{
    //    // add to the player's score
    //    score += 100;

    //    // update the score display
    //    scoreOverlay.text = score.ToString("0000");
    //}

    ///* GameState: after playing is over */

    //public void PlayerLost()
    //{
    //    // remove a life
    //    livesLeft--;

    //    // update the lives overlay
    //    livesOverlay.text = livesLeft.ToString();

    //    // check if player is out of lives
    //    if (livesLeft > 0)
    //    {
    //        // Make the player death sound
    //        SoundManager.S.PlayDeathSound();

    //        // go into oops state
    //        StartCoroutine(OopsState());
    //    }
    //    else
    //    {
    //        // go into game over (lose) state
    //        GameOverState(false);
    //    }
    //}

    //public IEnumerator OopsState()
    //{
    //    // go into oops state
    //    gameState = GameState.oops;

    //    // stop the game timer
    //    if (levelTimer != null)
    //    {
    //        StopCoroutine(levelTimer);
    //        levelTimer = null;
    //    }

    //    // turn on the oops message
    //    messageOverlay.enabled = true;
    //    messageOverlay.text = "Lives Left: " + livesLeft;

    //    // brief wait for the message & player destruction
    //    yield return new WaitForSeconds(4.0f);

    //    // turn the overlay message off
    //    messageOverlay.enabled = false;

    //    // reset the round from checkpoint
    //    LevelManager.S.RestartLevelCheckpoint();
    //}

    private IEnumerator LevelComplete()
    {
        // transition state while we wait after winning
        gameState = GameState.roundWin;

        // play victory fanfare
        if (AudioManager.S != null)
            AudioManager.S.Play("MysteriousJingle");

        // turn on the level complete message
        // messageOverlay.enabled = true;
        // messageOverlay.text = "Congratulations!\nYou Win!";

        // pause here for a moment
        yield return new WaitForSeconds(4.0f);

        if (LevelManager.S.finalScene)
        {
            // send back to main menu
            // messageOverlay.text = "Press the Back button\nto return to main menu";

            // turn on the back button
            // backButton.SetActive(true);
        }
        else
        {
            // go to the next round
            LevelManager.S.RoundWin();
        }
    }

    //private IEnumerator GameOverLose()
    //{
    //    // play the game over sound
    //    SoundManager.S.PlayGameoverClip();

    //    // turn on the game over message
    //    messageOverlay.enabled = true;

    //    messageOverlay.text = "You Lose...";
    //    // messageOverlay.text += "\nPress the Back button\nto return to main menu";
    //    messageOverlay.text += "\nFinal Score: " + score.ToString("0000");

    //    // pause here for a moment
    //    yield return new WaitForSeconds(4.0f);

    //    // turn on the back button
    //    // backButton.SetActive(true);

    //    // reset the level
    //    // LevelManager.S.RestartLevel();
    //}
}
