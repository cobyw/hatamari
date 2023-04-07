using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public Urchin urchin;
    public GameObject nextHatAttachPoint;
    private bool isAttached;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttached)
        {
            urchin.Attach(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        urchin = FindObjectOfType<Urchin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
