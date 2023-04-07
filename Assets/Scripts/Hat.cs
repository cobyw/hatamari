using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Hat : MonoBehaviour
{
    public GameObject nextHatAttachPoint;

    public SpriteRenderer spriteRenderer;

    public bool isAttached;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
