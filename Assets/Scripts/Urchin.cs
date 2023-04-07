using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour
{
    private List<Hat> hats = new List<Hat>();
    [SerializeField] private GameObject offsetPoint;

    private Hat topHat;

    private Vector3 offsetDistance;

    private void Start()
    {
        offsetDistance = offsetPoint.transform.position - transform.position;
    }

    private void Update()
    {
        offsetPoint.transform.position = transform.position + offsetDistance;
    }

    public void Attach(Hat hatToAttach)
    {
        hats.Add(hatToAttach);

        if (hats.Count == 1)
        {
            hatToAttach.gameObject.transform.parent = offsetPoint.transform;
            hatToAttach.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            hatToAttach.gameObject.transform.parent = topHat.nextHatAttachPoint.transform;
            hatToAttach.gameObject.transform.localPosition = Vector3.zero;
        }

        topHat = hatToAttach;
    }
}
