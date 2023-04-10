using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Flags]
public enum ScoreType
{
    None = 0,
    MaxHats = 1,
    TotalHats = 2,
    EndHats = 4,
}

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreType scoreType;
    [Multiline] [SerializeField] private string stringFormat = "value \n {0}";
    [SerializeField] private ScoreStructScriptable scoreScriptable;


    // Start is called before the first frame update
    void Start()
    {
        string value = "";

        switch (scoreType)
        {
            case (ScoreType.MaxHats):
                value = scoreScriptable.score.maxHats.ToString();
                break;
            case (ScoreType.TotalHats):
                value = scoreScriptable.score.totalHats.ToString();
                break;
            case (ScoreType.EndHats):
                value = scoreScriptable.score.endingHats.ToString();
                break;
        }

        GetComponent<TextMeshProUGUI>().text = string.Format(stringFormat, value);
    }
}
