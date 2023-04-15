using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private bool saveOnAwake = false;
    [SerializeField] private ScoreStructScriptable scoreScriptable;
    [SerializeField] int scoresToShow = 5;
    [SerializeField] private List<ScoreStruct> dummyScoreList;

    private List<ScoreStruct> scoreList;
    private ScoreStruct [] scoreArray;

    private ScoreStruct[] totalHatArray = new ScoreStruct[5];
    private ScoreStruct[] endHatArray = new ScoreStruct[5];
    private ScoreStruct[] maxHatArray = new ScoreStruct[5];

    private bool init;

    public ScoreStruct[] TotalHatArray
    {
        get
        {
            Init();
            return totalHatArray;
        }
    }

    public ScoreStruct[] EndHatArray
    {
        get
        {
            Init();
            return endHatArray;
        }
    }

    public ScoreStruct[] MaxHatArray
    {
        get
        {
            Init();
            return maxHatArray;
        }
    }

    private void Awake()
    {
        if (saveOnAwake)
        {
            SaveScores();
        }

        Init();
    }

    public void Init(bool force = false)
    {
        if (!init || force)
        {
            LoadScores();
            SetUpHighScores();
            init = true;
        }
    }

    public void SaveScores(bool clearScoreObject = false)
    {
        //make sure we have something before we save
        if (!scoreScriptable.isEmpty())
        {
            Init();

            //make sure we have a name set (no saving empties)
            if (!string.IsNullOrEmpty(scoreScriptable.score.initials))
            {
                //make sure this score isn't already saved
                if (!scoreScriptable.isSaved)
                {
                    scoreList.Add(scoreScriptable.score);
                    scoreArray = scoreList.ToArray();

                    var jsonToSaveConverted = JsonHelper.ToJson(scoreArray);

                    PlayerPrefs.SetString("SaveData", jsonToSaveConverted);
                    PlayerPrefs.Save();

                    Debug.Log("Saved in PlayerPrefs:" + jsonToSaveConverted);
                    scoreScriptable.isSaved = true;
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

            Init(force:true);
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
                scoreList = dummyScoreList;
            }
        }
        else
        {
            scoreList = dummyScoreList;
        }
    }

    private void SetUpHighScores()
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
            scoreList.Sort((s1, s2) => s1.endHats.CompareTo(s2.endHats));
            scoreList.Reverse();
            endHatArray = scoreList.GetRange(0, scoresToShow).ToArray();
        }
    }

    public ScoreType IsHighScore()
    {
        Init();

        ScoreType retval = ScoreType.None;

        if (totalHatArray[0] != null)
        {
            if (scoreScriptable.score.totalHats >= totalHatArray[0].totalHats || totalHatArray[0].Equals(scoreScriptable.score))
            {
                retval |= ScoreType.TotalHats;
            }
        }

        if (maxHatArray[0] != null)
        {
            if (scoreScriptable.score.maxHats >= maxHatArray[0].maxHats || maxHatArray[0].Equals(scoreScriptable.score))
            {
                retval |= ScoreType.MaxHats;
            }
        }

        if (endHatArray[0] != null)
        {
            if (scoreScriptable.score.endHats >= endHatArray[0].endHats || endHatArray[0].Equals(scoreScriptable.score))
            {
                retval |= ScoreType.EndHats;
            }
        }

        return retval;
    }

    public ScoreType IsRecordableScore()
    {
        Init();

        ScoreType retval = ScoreType.None;

        foreach (ScoreStruct scoreStruct in totalHatArray)
        {
            if (scoreStruct != null)
            {
                if (scoreStruct.Equals(scoreScriptable.score) || scoreScriptable.score.totalHats >= scoreStruct.totalHats)
                {
                    retval |= ScoreType.TotalHats;
                }
            }
        }

        foreach (ScoreStruct scoreStruct in maxHatArray)
        {
            if (scoreStruct != null)
            {
                if (scoreStruct.Equals(scoreScriptable.score) || scoreScriptable.score.maxHats >= scoreStruct.maxHats)
                {
                    retval |= ScoreType.MaxHats;
                }
            }
        }

        foreach (ScoreStruct scoreStruct in endHatArray)
        {
            if (scoreStruct != null)
            {
                if (scoreStruct.Equals(scoreScriptable.score) || scoreScriptable.score.endHats >= scoreStruct.endHats)
                {
                    retval |= ScoreType.EndHats;
                }
            }
        }

        return retval;
    }

    public string GetMostReventInitials()
    {
        Init();

        var retval = "";
        var mostRecentScore = scoreList.OrderByDescending(x => x.time).FirstOrDefault();

        if (mostRecentScore.time != (int)System.DateTime.MinValue.Ticks)
        {
            retval = mostRecentScore.initials;
        }

        Debug.Log(retval);
        return retval;
    }

    [Button]
    public void ClearCurrntScore()
    {
        scoreScriptable.Clear();
    }

    [Button]
    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
        scoreScriptable.isSaved = false;
    }


    [Button]
    public void StoreDummyData()
    {
        ScoreStruct[] array = dummyScoreList.ToArray();

        var jsonToSaveConverted = JsonHelper.ToJson(array);

        PlayerPrefs.SetString("SaveData", jsonToSaveConverted);
        PlayerPrefs.Save();

        Debug.Log("Saved in PlayerPrefs:" + jsonToSaveConverted);
    }

    [Button]
    public void PrintPlayerPrefs()
    {
        Debug.Log("PlayerPrefs:" + PlayerPrefs.GetString("SaveData"));
    }
}
