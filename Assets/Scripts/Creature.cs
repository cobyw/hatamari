using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creature : MonoBehaviour
{
    public List<Hat> creatureHats = new List<Hat>();
    public float speed = 1;
    public GameObject attachPoint;

    [SerializeField] private bool winCreature = false;
    [SerializeField] private float winTimeScale = 0.25f;

    [SerializeField] private float timeToLoadWinScreen;
    [SerializeField] private float timeToLoadLoseScreen;

    [SerializeField] private string winScreenName = "winScreen";

    [SerializeField] private float loseTimeScale = 0.5f;
    [SerializeField] private string loseScreenName = "loseScreen";

    private Vector3 movementVector;

    private Urchin urchin;

    private void Start()
    {
        Cursor.visible = false;
        urchin = FindObjectOfType<Urchin>();

        movementVector = new Vector3(speed, 0, 0);

        if (attachPoint == null)
        {
            attachPoint = gameObject;
            Debug.LogWarning("No attachPoint set on " + gameObject);
        }

        timeToLoadWinScreen *= winTimeScale;
        timeToLoadLoseScreen *= loseTimeScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += movementVector * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == urchin.gameObject)
        {
            if (winCreature)
            {
                Win();
            }
            else
            {
                Attack(collision.gameObject);
            }
        }
        else if (collision.GetComponent<Hat>() != null)
        {
            if (!winCreature)
            {
                Attach(collision.GetComponent<Hat>());
            }
            else
            {
                //winners only need one hat
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void Attack(GameObject objectHit)
    {
        if (urchin.UrchinHats.Count == 0)
        {
            Lose();
        }
        else
        {
            Attach(urchin.UrchinHats[0]);
            urchin.UrchinHats.Clear();
        }
    }


    public void Attach(Hat hatToAttach)
    {
        if (hatToAttach.isAttached)
        {
            if (urchin.UrchinHats.Contains(hatToAttach))
            {
                var indexOfHat = urchin.UrchinHats.FindIndex(x => x.Equals(hatToAttach));

                if (indexOfHat == 0)
                {
                    urchin.UrchinHats.Clear();
                }
                else
                {
                    urchin.UrchinHats = urchin.UrchinHats.GetRange(0, indexOfHat);
                }
            }
            //if we are attached but not to the urchin return early. Creatures do not steal hats from one another.
            else
            {
                return;
            }
        }

        if (creatureHats.Count == 0)
        {
            hatToAttach.gameObject.transform.parent = attachPoint.transform;
            hatToAttach.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            hatToAttach.gameObject.transform.parent = creatureHats[creatureHats.Count - 1].nextHatAttachPoint.transform;
            hatToAttach.gameObject.transform.localPosition = Vector3.zero;
        }

        creatureHats.Add(hatToAttach);

        hatToAttach.spriteRenderer.sortingOrder = creatureHats.Count;

        hatToAttach.isAttached = true;
    }

    private void Win()
    {
        //update the end score
        urchin.UpdateEndScore();

        //slows down time to indicate winning
        Time.timeScale = winTimeScale;

        //puts the urchin on the crab as a hat and freezes them
        urchin.transform.parent = attachPoint.transform;
        urchin.transform.localPosition = Vector3.zero;
        urchin.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        urchin.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        //gross but whatever - makes it so creatures won't steal hats
        foreach (CircleCollider2D circleCollider in FindObjectsOfType<CircleCollider2D>())
        {
            circleCollider.enabled = false;
        }

        //also gross but whatever - makes it so things stop spawning
        foreach (Spawner spawner in FindObjectsOfType<Spawner>())
        {
            spawner.StopSpawningOverTime();
        }

        StartCoroutine(LoadWinScreen());
    }

    IEnumerator LoadWinScreen()
    {
        yield return new WaitForSeconds(timeToLoadWinScreen);
        Time.timeScale = 1;
        Cursor.visible = true;
        SceneManager.LoadScene(winScreenName);
    }

    private void Lose()
    {
        //slows down time to indicate losing
        Time.timeScale = loseTimeScale;

        urchin.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        urchin.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        StartCoroutine(LoseOverTime());
    }

    IEnumerator LoseOverTime()
    {
        var timeLosing = 0f;
        var scale = 1f;
        while (timeLosing < timeToLoadLoseScreen)
        {
            timeLosing += Time.deltaTime;
            scale = Mathf.Lerp(1, 0, timeLosing / timeToLoadLoseScreen);
            urchin.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Time.timeScale = 1;
        Cursor.visible = true;
        SceneManager.LoadScene(loseScreenName);
    }

}
