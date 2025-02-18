using UnityEngine;

public static class GameManager
{
    public static GameObject reverseCollectible = GameObject.Find("ReverseCollectible");
    public enum GameState {
        // A paused state, exited with spacebar.
        Waiting,

        // Normal running mode.
        NormalMode,

        // Camera runs backwards.
        ReverseMode,

        // Allows us to do a little cool animation to transition to reverse mode.
        Transitioning,
        GameOver
    }
    public static GameState gameState = GameState.Waiting;
    public static Vector3 cameraOffset = Constants.CAMERA_FOLLOW_OFFSET_NORMAL;

    public static void EnterWaitingMode() {
        gameState = GameState.Waiting;
        Time.timeScale = 0;
        cameraOffset = Constants.CAMERA_FOLLOW_OFFSET_NORMAL;
        TraceSpawnerManager traceSpawnerManager = GameObject.Find("TraceSpawner").GetComponent<TraceSpawnerManager>();
        traceSpawnerManager.ClearTraces();
    }

        public static void EnterGameOverMode() {
        gameState = GameState.GameOver;
        Time.timeScale = 0;
    }
    
    public static void EnterNormalMode() {
        gameState = GameState.NormalMode;
        reverseCollectible.SetActive(true);
        Time.timeScale = 1;
    }
    public static void EnterTransitionMode() {
        reverseCollectible.SetActive(false);
        gameState = GameState.Transitioning;
        cameraOffset = Constants.CAMERA_FOLLOW_OFFSET_REVERSE;
        Time.timeScale = 0;
        TraceSpawnerManager traceSpawnerManager = GameObject.Find("TraceSpawner").GetComponent<TraceSpawnerManager>();
        traceSpawnerManager.traceCooldownText.text = "";
    }

    public static void EnterReverseMode() {
        gameState = GameState.ReverseMode;
        Time.timeScale = 1;
    }
}