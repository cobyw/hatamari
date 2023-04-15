using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ScoreInitialSaver : MonoBehaviour
{
    [SerializeField] private string validMessage = "Press Enter to Submit";
    [SerializeField] private string highScoreMessage = "HIGH SCORE! Enter your initials!";
    [SerializeField] private string newScoreMessage = "GREAT SCORE! Enter your initials!";
    [SerializeField] private string invalidMessage = "Initials must be less than 3 characters!";

    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI informationalMessage;
    [SerializeField] private ScoreManager highScoreManager;
    [SerializeField] private SceneLoader sceneLoader;

    private ScoreType recordableScore = ScoreType.None;
    private ScoreType highScore = ScoreType.None;

    private bool isRecordableScore;
    private bool isHighScore;

    private bool invalidInitials = true;

    private void Start()
    {
        if (inputField == null)
        {
            inputField = FindObjectOfType<TMP_InputField>();
        }

        if (sceneLoader == null)
        {
            sceneLoader = FindObjectOfType<SceneLoader>();
        }


        recordableScore = highScoreManager.IsRecordableScore();
        highScore = highScoreManager.IsHighScore();

        //determine if any of our scores are good enough to record
        isRecordableScore = recordableScore.HasFlag(ScoreType.MaxHats) || recordableScore.HasFlag(ScoreType.EndHats) || recordableScore.HasFlag(ScoreType.TotalHats);
        isHighScore = highScore.HasFlag(ScoreType.MaxHats) || highScore.HasFlag(ScoreType.EndHats) || highScore.HasFlag(ScoreType.TotalHats);

        //if we aren't good enough don't show the intials field
        if (isHighScore)
        {
            informationalMessage.text = highScoreMessage;
        }
        else if (isRecordableScore)
        {
            informationalMessage.text = newScoreMessage;
        }
        else
        {
            inputField.gameObject.SetActive(false);
            informationalMessage.text = "";
            sceneLoader.EnableSceneChange();
        }
    }

    private void ValidateInitials()
    {
        //uppercase
        inputField.text = inputField.text.ToUpper();

        //must have a string less than 3 characters and with text
        invalidInitials = (inputField.text.Length > 3 || string.IsNullOrEmpty(inputField.text));

        //determine which message to display mased on initials
        informationalMessage.text = invalidInitials ? invalidMessage : validMessage;
    }

    public void SaveInitials()
    {
        ValidateInitials();

        if (!invalidInitials)
        {
            scoreScriptable.score.initials = inputField.text;
        }
    }

    public void TrySubmit()
    {
        SaveInitials();

        if (!invalidInitials)
        {
            sceneLoader.EnableSceneChange();
        }
    }
}
