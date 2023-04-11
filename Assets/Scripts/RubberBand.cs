using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberBand : MonoBehaviour
{
    [SerializeField] private GameObject rubberBandObject;
    [SerializeField] private Vector3 jumpVector = new Vector3(1, 0, 0);
    [SerializeField] private float minOriginalDistanceToActivate = 0.2f;
    [SerializeField] private float maxOriginalDistanceToActivate = 2;
    [SerializeField] private bool startSpawnsOnActivation = true;

    [SerializeField] private SpawnerManager spawnerManager;

    private float originalDistance;


    private float minDistanceToActivate;
    private float maxDistanceToActivate;

    private float currentDistance;
    private bool active = false;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        originalDistance = Vector3.Distance(rubberBandObject.transform.position, originalPosition);

        minDistanceToActivate = minOriginalDistanceToActivate * originalDistance;
        maxDistanceToActivate = maxOriginalDistanceToActivate * originalDistance;
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = Vector3.Distance(rubberBandObject.transform.position, transform.position);

        if (active)
        {
            if (currentDistance  > originalDistance)
            {
                transform.position += jumpVector;
            }
        }
        else
        {
            //are we far enough or close enough away to activate?
            if (currentDistance > maxDistanceToActivate || currentDistance < minDistanceToActivate)
            {
                active = true;
                if (startSpawnsOnActivation)
                {
                    spawnerManager.StartSpawn();
                }
            }
            else
            {
                //don't let them move if they aren't active
                transform.position = originalPosition;
            }
        }
    }
}
