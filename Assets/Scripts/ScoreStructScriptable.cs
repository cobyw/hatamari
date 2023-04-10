using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreStruct
{
    public int totalHats;
    public int endingHats;
    public int maxHats;
    public string initials;
    public System.DateTime time;
}

[CreateAssetMenu(fileName = "High Score Object", menuName = "ScriptableObjects/High Score", order = 1)]
public class ScoreStructScriptable : ScriptableObject
{
    public ScoreStruct score;

    /// <summary>
    /// determines if the high score should be saved 
    /// </summary>
    /// <returns>true if either the total, ending, or max hats are empty.
    public bool isEmpty()
    {
        return (score.totalHats == 0 && score.endingHats == 0 && score.maxHats == 0);
    }

    public void Clear()
    {
        score = new ScoreStruct();
    }
}