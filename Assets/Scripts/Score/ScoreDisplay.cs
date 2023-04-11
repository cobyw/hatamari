using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreType scoreType;
    [Multiline] [SerializeField] private string stringFormat = "value \n {0}";
    [SerializeField] private bool keepUpToDate;
    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        if (textMeshProUGUI == null)
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        textMeshProUGUI.text = string.Format(stringFormat, scoreScriptable.GetScoreValue(scoreType));
    }

    private void Update()
    {
        if (keepUpToDate)
        {
            textMeshProUGUI.text = string.Format(stringFormat, scoreScriptable.GetScoreValue(scoreType));
        }
    }
}
