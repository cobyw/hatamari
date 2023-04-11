using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RecordScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreType scoreType;
    [Multiline] [SerializeField] private string highScoreFormat = "HIGH SCORE\n {0}";
    [Multiline] [SerializeField] private string recordScoreFormat = "RECORD SCORE\n {0}";
    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] private ScoreManager scoreManager;

    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        scoreManager.Init();

        var highsScoreType = scoreManager.IsHighScore();
        var recordScoreType = scoreManager.IsRecordableScore();

        if (highsScoreType.HasFlag(scoreType))
        {
            GetComponent<TextMeshProUGUI>().text = string.Format(highScoreFormat, scoreScriptable.GetScoreValue(scoreType));
        }
        else if (recordScoreType.HasFlag(scoreType))
        {
            GetComponent<TextMeshProUGUI>().text = string.Format(recordScoreFormat, scoreScriptable.GetScoreValue(scoreType));
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
