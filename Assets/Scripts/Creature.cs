using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public List<Hat> creatureHats = new List<Hat>();
    public float speed = 1;
    public GameObject attachPoint;

    public bool winCreature = false;

    private Vector3 movementVector;

    private Urchin urchin;

    private void Start()
    {
        urchin = FindObjectOfType<Urchin>();

        movementVector = new Vector3(speed, 0, 0);

        if (attachPoint == null)
        {
            attachPoint = gameObject;
            Debug.LogWarning("No attachPoint set on " + gameObject);
        }
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
            Attach(collision.GetComponent<Hat>());
        }
    }

    private void Attack(GameObject objectHit)
    {
        if (urchin.urchinHats.Count == 0)
        {
            Lose();
        }
        else
        {
            Attach(urchin.urchinHats[0]);
            urchin.urchinHats.Clear();
        }
    }

    private void Win()
    {
        Debug.Log("YOU WIN");
    }

    private void Lose()
    {
        Debug.Log("YOU LOSE");
    }

    public void Attach(Hat hatToAttach)
    {
        if (hatToAttach.isAttached)
        {
            if (urchin.urchinHats.Contains(hatToAttach))
            {
                var indexOfHat = urchin.urchinHats.FindIndex(x => x.Equals(hatToAttach));

                if (indexOfHat == 0)
                {
                    urchin.urchinHats.Clear();
                }
                else
                {
                    urchin.urchinHats = urchin.urchinHats.GetRange(0, indexOfHat);
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
}
