using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    [SerializeField] private GameObject rubberBandObject;
    [SerializeField] private Vector3 jumpVector = new Vector3(1, 0, 0);

    private float maxDistance;

    private void Start()
    {
        maxDistance = Vector3.Distance(rubberBandObject.transform.position, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(rubberBandObject.transform.position, transform.position) > maxDistance)
        {
            transform.position += jumpVector;
        }
    }
}
