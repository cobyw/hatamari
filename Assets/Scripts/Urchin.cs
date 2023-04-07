using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour
{
    public List<Hat> urchinHats = new List<Hat>();
    [SerializeField] private GameObject offsetPoint;

    private Vector3 offsetDistance;

    private void Start()
    {
        offsetDistance = offsetPoint.transform.position - transform.position;
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
    }
}
