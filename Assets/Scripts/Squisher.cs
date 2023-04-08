using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squisher : MonoBehaviour
{
    public float timeToSquish;
    public float timeToReturn;

    public float squishSize = .2f;
    public float normalSize = 1;

    public GameObject parentToSquish;

    public bool squishing = false;

    private float timeSpentSquishing = 0;
    private float timeSpentReturning = 0;

    private float originalReturnScale = 0;
    private float originalSquishScale = 0;

    private float newSize = 1f;

    private void FixedUpdate()
    {
        if (squishing)
        {
            timeSpentReturning = 0;
            //we have room to squish
            if (parentToSquish.transform.localScale.y != squishSize)
            {
                if (timeSpentReturning == 0)
                {
                    originalSquishScale = parentToSquish.transform.localScale.y;
                }


                timeSpentSquishing += Time.fixedDeltaTime;
                if (timeSpentSquishing < timeToSquish)
                {
                    newSize = Mathf.Lerp(originalSquishScale, squishSize, timeSpentSquishing / timeToSquish);
                }
                else
                {
                    newSize = squishSize;
                    squishing = false;
                }

                parentToSquish.transform.localScale = new Vector3(parentToSquish.transform.localScale.x, newSize, parentToSquish.transform.localScale.z);
            }
            else
            {
                squishing = false;
            }
        }
        else
        {
            timeSpentSquishing = 0;
            if (parentToSquish.transform.localScale.y != normalSize)
            {
                if (timeSpentReturning == 0)
                {
                    originalReturnScale = parentToSquish.transform.localScale.y;
                }

                timeSpentReturning += Time.fixedDeltaTime;

                if (timeSpentReturning < timeToReturn)
                {
                    newSize = Mathf.Lerp(originalReturnScale, normalSize, timeSpentReturning / timeToReturn);
                }
                else
                {
                    newSize = normalSize;
                }

                parentToSquish.transform.localScale = new Vector3(parentToSquish.transform.localScale.x, newSize, parentToSquish.transform.localScale.z);
            }
        }
    }
}
