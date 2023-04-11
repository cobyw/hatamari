using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FinalScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreType scoreType;
    [Multiline] [SerializeField] private string titleString = "";
    [Multiline] [SerializeField] private string headerString = "\nINITIALS SCORE";
    [Multiline] [SerializeField] private string stringFormat = "\n{0} {1}";


    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] private ScoreManager scoreManager;

    private string output = "";

    // Start is called before the first frame update
    void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        scoreManager.Init();
        SetUpScores();
    }


    void SetUpScores()
    {
        output += titleString;
        output += headerString;

        string initals = "";
        string value = "";

        switch (scoreType)
        {
            case (ScoreType.MaxHats):
                foreach (ScoreStruct score in scoreManager.MaxHatArray)
                {
                    initals = score.initials;
                    value = score.maxHats.ToString();
                    output += string.Format(stringFormat, initals, value);
                }
                break;
            case (ScoreType.TotalHats):
                foreach (ScoreStruct score in scoreManager.TotalHatArray)
                {
                    initals = score.initials;
                    value = score.totalHats.ToString();
                    output += string.Format(stringFormat, initals, value);
                }
                break;
            case (ScoreType.EndHats):
                foreach (ScoreStruct score in scoreManager.EndHatArray)
                {
                    initals = score.initials;
                    value = score.endHats.ToString();
                    output += string.Format(stringFormat, initals, value);
                }
                break;
        }

        GetComponent<TextMeshProUGUI>().text = output;
    }
}
