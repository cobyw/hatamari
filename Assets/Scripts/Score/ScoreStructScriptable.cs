using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreStruct
{
    public int totalHats;
    public int endHats;
    public int maxHats;
    public string initials;
    public System.DateTime time;

    public bool Equals(ScoreStruct other)
    {
        bool retval = true;

        retval &= totalHats == other.totalHats;
        retval &= endHats == other.endHats;
        retval &= maxHats == other.maxHats;
        retval &= initials == other.initials;
        retval &= time == other.time;

        return retval;
    }
}

[System.Flags]
public enum ScoreType
{
    None = 0,
    MaxHats = 1,
    TotalHats = 2,
    EndHats = 4,
    CurrentHats = 8,
}

[CreateAssetMenu(fileName = "High Score Object", menuName = "ScriptableObjects/High Score", order = 1)]
public class ScoreStructScriptable : ScriptableObject
{
    public ScoreStruct score;
    public bool isSaved = false;
    public int currentHats = 0;

    /// <summary>
    /// determines if the high score should be saved 
    /// </summary>
    /// <returns>true if either the total, ending, or max hats are empty.
    public bool isEmpty()
    {
        return (score.totalHats == 0 && score.endHats == 0 && score.maxHats == 0);
    }

    public void Clear()
    {
        if (!isSaved)
        {
            Debug.Log("unsaved score scriptable cleared");
        }

        score = new ScoreStruct();
        currentHats = 0;
        isSaved = false;
    }

    public string GetScoreValue(ScoreType scoreType)
    {
        string value = "";

        switch (scoreType)
        {
            case (ScoreType.MaxHats):
                value = score.maxHats.ToString();
                break;
            case (ScoreType.TotalHats):
                value = score.totalHats.ToString();
                break;
            case (ScoreType.EndHats):
                value = score.endHats.ToString();
                break;
            case (ScoreType.CurrentHats):
                value = currentHats.ToString();
                break;
        }

        return value;
    }
}