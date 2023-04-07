using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tinter : MonoBehaviour
{
    [Header("Color Fields")]
    public Gradient colorGradient;

    public void Start()
    {
        GetComponent< SpriteRenderer>().color = colorGradient.Evaluate(Random.Range(0f, 1f));
    }

}
