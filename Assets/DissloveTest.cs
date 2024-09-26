using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissloveTest : MonoBehaviour
{
    public Material material;
    public float amount = -1;
    public bool bDisslove;
    // Start is called before the first frame update
    private void Start()
    {
        amount = -1;
        material.SetFloat("_Amount", amount);
    }

    // Update is called once per frame
    void Update()
    {
        if(bDisslove)
        {
            amount += Time.deltaTime;
            material.SetFloat("_Amount", amount);
        }
    }
}
