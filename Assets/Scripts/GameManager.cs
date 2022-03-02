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

    // Gameplay variables for the caterpillar levels
    public int totalLeafCount;
    private int leafCount;

    // TODO: Move these to a LevelManager script
    public GameObject butterflyPrefab;

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

    private void Update()
    {
        // Use the R key to restart the level
        if (Input.GetKeyDown(KeyCode.R))
            LevelManager.S.RestartLevel();
    }

    /* Used to start the game/round */

    public void InitializeNewGame()
    {
        // reset game variables
        leafCount = 0;

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

    public void CollectLeaf()
    {
        // The player has collected a leaf, add to their total
        leafCount++;

        // Make the player bigger
        if (leafCount < totalLeafCount)
            PlayerController.player.GetBigger();

        // Transform into a butterfly
        if (leafCount >= totalLeafCount)
            Metamorphosis();
    }

    public void TriggerCheckpoint()
    {
        // The player has reached a checkpoint
        // For now, the only checkpoint is at the end of the level
        Debug.Log("You win!");
    }

    public void Metamorphosis()
    {
        // Remove the caterpillar version of the player
        Vector3 playerPosition = PlayerController.player.gameObject.transform.position;
        Destroy(PlayerController.player.gameObject);

        // Spawn the butterfly version of the player in the same place
        GameObject flyPlayer = Instantiate(butterflyPrefab);
        flyPlayer.transform.position = playerPosition;
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

    //public void PlayerKilledEnemy()
    //{
    //    // add to the player's score
    //    score += 200;

    //    // update the score display
    //    scoreOverlay.text = score.ToString("0000");
    //}

    //public void PlayerCollectedCoin()
    //{
    //    // add to the player's score
    //    score += 100;

    //    // update the score display
    //    scoreOverlay.text = score.ToString("0000");
    //}

    //public void PlayerCollectedOneUp()
    //{
    //    // give the player an extra life
    //    livesLeft++;

    //    // update the lives display
    //    livesOverlay.text = livesLeft.ToString();
    //}

    //public void PlayerReachedCheckpoint()
    //{
    //    // tell LevelManager that player hit checkpoint
    //    LevelManager.S.SetCheckpoint(true);

    //    // display checkpoint message
    //    StartCoroutine(CheckpointMessage());
    //}

    //public IEnumerator CheckpointMessage()
    //{
    //    // turn on the checkpoint message
    //    messageOverlay.enabled = true;
    //    messageOverlay.text = "Checkpoint Reached";

    //    // pause here for a moment
    //    yield return new WaitForSeconds(2.0f);

    //    // turn off the checkpoint message
    //    messageOverlay.enabled = false;
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

    //public void GameOverState(bool victory)
    //{
    //    // go into gameover state
    //    gameState = GameState.gameOver;

    //    // stop the game timer
    //    if (levelTimer != null)
    //    {
    //        StopCoroutine(levelTimer);
    //        levelTimer = null;
    //    }

    //    // diverges here based on whether the player won
    //    if (victory)
    //        StartCoroutine(LevelComplete());
    //    else
    //        StartCoroutine(GameOverLose());
    //}

    //private IEnumerator LevelComplete()
    //{
    //    // transition state while we wait after winning
    //    gameState = GameState.roundWin;

    //    // play victory fanfare
    //    SoundManager.S.PlayFanfareClip();

    //    // turn on the level complete message
    //    messageOverlay.enabled = true;
    //    messageOverlay.text = "Congratulations!\nYou Win!";

    //    // pause here for a moment
    //    yield return new WaitForSeconds(4.0f);

    //    if (LevelManager.S.finalScene)
    //    {
    //        // send back to main menu
    //        // messageOverlay.text = "Press the Back button\nto return to main menu";

    //        // turn on the back button
    //        // backButton.SetActive(true);
    //    }
    //    else
    //    {
    //        // go to the next round
    //        LevelManager.S.RoundWin();
    //    }
    //}

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
