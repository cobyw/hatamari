using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour
{
    public List<Hat> urchinHats = new List<Hat>();
    [SerializeField] private GameObject offsetPoint;
    [SerializeField] private ScoreStructScriptable scoreStructScriptable;
    [SerializeField] private ScoreManager highScoreManager;

    private Vector3 offsetDistance;

    private void Start()
    {
        offsetDistance = offsetPoint.transform.position - transform.position;

        if (highScoreManager == null)
        {
            highScoreManager = FindObjectOfType<ScoreManager>();
        }
        //clears the current score
        scoreStructScriptable.Clear();

        //stores the new time associated with the save
        scoreStructScriptable.score.time = System.DateTime.Now;
    }

    private void Update()
    {
        offsetPoint.transform.position = transform.position + offsetDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Hat>())
        {
            Attach(collision.GetComponent<Hat>());
        }
    }

    public void Attach(Hat hatToAttach)
    {
        int initialCount = urchinHats.Count;

        if (!hatToAttach.isAttached)
        {
            if (urchinHats.Count == 0)
            {
                hatToAttach.gameObject.transform.parent = offsetPoint.transform;
                hatToAttach.gameObject.transform.localPosition = Vector3.zero;
            }
            else
            {
                hatToAttach.gameObject.transform.parent = urchinHats[urchinHats.Count - 1].nextHatAttachPoint.transform;
                hatToAttach.gameObject.transform.localPosition = Vector3.zero;
            }
            urchinHats.Add(hatToAttach);

            hatToAttach.spriteRenderer.sortingOrder = urchinHats.Count;

            hatToAttach.isAttached = true;
        }

        //if we actually gained any hats add the hat
        if (urchinHats.Count != initialCount)
        {
            scoreStructScriptable.score.totalHats++;
        }

        //if we have more hats than our previous max this is now our max.
        scoreStructScriptable.score.maxHats = Mathf.Max(urchinHats.Count, scoreStructScriptable.score.maxHats);
    }

    public void UpdateEndScore()
    {
        scoreStructScriptable.score.endingHats = urchinHats.Count;
    }
}
