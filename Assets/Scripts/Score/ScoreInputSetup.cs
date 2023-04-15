using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreInputSetup : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        if (inputField == null)
        {
            inputField = GetComponent< TMP_InputField>();
        }

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        if (scoreManager != null)
        {
            inputField.text = scoreManager.GetMostReventInitials();
        }

        if (inputField != null)
        {
            inputField.Select();
            inputField.ActivateInputField();
            inputField.characterLimit = 3;
        }
    }
}
