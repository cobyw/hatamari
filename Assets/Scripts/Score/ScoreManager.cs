using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] int scoresToShow = 5;
    [SerializeField] private List<ScoreStruct> dummyScoreList;

    private List<ScoreStruct> scoreList;
    private ScoreStruct [] scoreArray;

    private ScoreStruct[] totalHatArray = new ScoreStruct[5];
    private ScoreStruct[] endHatArray = new ScoreStruct[5];
    private ScoreStruct[] maxHatArray = new ScoreStruct[5];

    private string yourScoreString;
    private string highScoreString;

    public void Save(bool clearScoreObject = false)
    {
        //make sure we have something before we save
        if (!scoreScriptable.isEmpty())
        {
            LoadScores();

            //make sure we have a name set (no saving empties)
            if (!string.IsNullOrEmpty(scoreScriptable.score.initials))
            {
                //make sure this score isn't already saved
                if (!scoreList.Contains(scoreScriptable.score))
                {
                    scoreList.Add(scoreScriptable.score);

                    scoreArray = scoreList.ToArray();

                    var jsonToSaveConverted = JsonUtility.ToJson(scoreArray);



                    PlayerPrefs.SetString("SaveData", jsonToSaveConverted);

                    Debug.Log("Saved in PlayerPrefs:" + jsonToSaveConverted);
                }
                else
                {
                    Debug.LogError("tried to save the same struct more than once. " + scoreScriptable.score.time);
                }
            }
            else
            {
                Debug.LogError("tried to save a struct with no initials " + scoreScriptable.score.time);
            }

            //clear the struct
            if (clearScoreObject)
            {
                scoreScriptable.Clear();
            }
        }
        else
        {
            Debug.LogError("tried to save empty score.");
        }
    }

    private void LoadScores()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            Debug.Log("Found in PlayerPrefs:" + PlayerPrefs.GetString("SaveData"));

            scoreArray = JsonHelper.FromJson<ScoreStruct>(PlayerPrefs.GetString("SaveData"));

            if (scoreArray != null && scoreArray.Length != 0)
            {
                scoreList = scoreArray.ToList();
            }
            else
            {
                scoreList = new List<ScoreStruct>();
            }
        }
        else
        {
            scoreList = new List<ScoreStruct>();
        }
    }

    private void GetHighScores()
    {
        if (scoreList.Count != 0)
        {
            //get the total hats
            scoreList.Sort((s1, s2) => s1.totalHats.CompareTo(s2.totalHats));
            scoreList.Reverse();
            totalHatArray = scoreList.GetRange(0, scoresToShow).ToArray();

            //get the max hats
            scoreList.Sort((s1, s2) => s1.maxHats.CompareTo(s2.maxHats));
            scoreList.Reverse();
            maxHatArray = scoreList.GetRange(0, scoresToShow).ToArray();

            //get the end hats
            scoreList.Sort((s1, s2) => s1.endingHats.CompareTo(s2.endingHats));
            scoreList.Reverse();
            endHatArray = scoreList.GetRange(0, scoresToShow).ToArray();
        }
    }

    public ScoreType IsHighScore()
    {
        LoadScores();
        GetHighScores();

        ScoreType retval = ScoreType.None;

        int maxHatHighScore = 0;
        int totalHatHighScore = 0;
        int endHatHighScore = 0;

        //determine the highest score that is in the best scores
        foreach (ScoreStruct scoreStruct in totalHatArray)
        {
            if (scoreStruct != null)
            {
                maxHatHighScore = Mathf.Max(scoreStruct.maxHats, maxHatHighScore);
            }
        }


        foreach (ScoreStruct scoreStruct in maxHatArray)
        {
            if (scoreStruct != null)
            {
                totalHatHighScore = Mathf.Max(scoreStruct.totalHats, totalHatHighScore);
            }
        }


        foreach (ScoreStruct scoreStruct in endHatArray)
        {
            if (scoreStruct != null)
            {
                endHatHighScore = Mathf.Max(scoreStruct.endingHats, endHatHighScore);
            }
        }

        //see if we are higher than that score

        if (scoreScriptable.score.maxHats > maxHatHighScore)
        {
            retval |= ScoreType.MaxHats;
        }

        if (scoreScriptable.score.totalHats > totalHatHighScore)
        {
            retval |= ScoreType.TotalHats;
        }

        if (scoreScriptable.score.endingHats > endHatHighScore)
        {
            retval |= ScoreType.EndHats;
        }

        return retval;
    }

    public ScoreType IsRecordableScore()
    {
        LoadScores();
        GetHighScores();

        ScoreType retval = ScoreType.None;

        int maxHatMinScore = int.MaxValue;
        int totalHatMinScore = int.MaxValue;
        int endHatMinScore = int.MaxValue;

        //determine the lowest score that is in the best scores
        foreach (ScoreStruct scoreStruct in totalHatArray)
        {
            if (scoreStruct != null)
            {
                maxHatMinScore = Mathf.Min(scoreStruct.maxHats, maxHatMinScore);
            }
        }


        foreach (ScoreStruct scoreStruct in maxHatArray)
        {
            if (scoreStruct != null)
            {
                totalHatMinScore = Mathf.Min(scoreStruct.totalHats, totalHatMinScore);
            }
        }


        foreach (ScoreStruct scoreStruct in endHatArray)
        {
            if (scoreStruct != null)
            {
                endHatMinScore = Mathf.Min(scoreStruct.endingHats, endHatMinScore);
            }
        }


        if (scoreScriptable.score.maxHats > maxHatMinScore || maxHatMinScore == int.MaxValue)
        {
            retval |= ScoreType.MaxHats;
        }

        if (scoreScriptable.score.totalHats > totalHatMinScore || totalHatMinScore == int.MaxValue)
        {
            retval |= ScoreType.TotalHats;
        }

        if (scoreScriptable.score.endingHats > endHatMinScore || endHatMinScore == int.MaxValue)
        {
            retval |= ScoreType.EndHats;
        }

        return retval;
    }
}