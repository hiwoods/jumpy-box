using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;
using Unity.Collections;

[AlwaysUpdateSystem]
public class UpdateHUDSystem : ComponentSystem {

    struct PlayerData
    {
        public int Length;
        [ReadOnly] public ComponentArray<Transform> Transforms;
        [ReadOnly] public ComponentArray<PlayerState> PlayerStates;
    }

    [Inject] private PlayerData PlayerEntities;

    private GameObject StartText;
    private GameObject LoseText;
    private UIEventHandlers UIHandler;
    private Text Score;

    private int score;
    private int accumulatedScoreIn1Jump;

    private static string UNIT = "m";

    public void SetupGameObjects()
    {
        UIHandler = GameObject.Find("Canvas").GetComponent<UIEventHandlers>();
        StartText = GameObject.Find("Canvas/StartText");
        LoseText = GameObject.Find("Canvas/LoseText");
        Score = GameObject.Find("Canvas/Score").GetComponent<Text>();

        UpdateReset();
    }

    protected override void OnUpdate()
    {
        //player destroyed, enable click event to reset
        if (PlayerEntities.Length == 0)
        {
            UIHandler.enabled = true;
            return;
        }

        //show lose text once dead
        if (PlayerEntities.PlayerStates[0].IsDead)
        {
            LoseText.SetActive(true);
            return;
        }

        //else update score based on player position
        var playerTransform = PlayerEntities.Transforms[0];
        var playerState = PlayerEntities.PlayerStates[0];
        ISettings settings = Bootstrap.Settings;

        if (playerState.Jumping)
        {
            accumulatedScoreIn1Jump = (int)(playerTransform.position.y - settings.MinY);
        }
        else
        {
            score += accumulatedScoreIn1Jump;
            accumulatedScoreIn1Jump = 0;
        }

        SetScore(score + accumulatedScoreIn1Jump);
    }

    private void SetScore(int score)
    {
        Score.text = $"{score} {UNIT}";
    }

    public void UpdateStart()
    {
        StartText.SetActive(false);
        LoseText.SetActive(false);
        score = 0;
    }

    public void UpdateReset()
    {
        StartText.SetActive(true);
        LoseText.SetActive(false);
        Score.text = $"0 {UNIT}";
        score = 0;
        accumulatedScoreIn1Jump = 0;
        UIHandler.enabled = true;
    }
    
}
