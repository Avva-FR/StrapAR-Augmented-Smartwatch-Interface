using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSelectorControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
    }

    public void activateOne()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
    }

    public void activateTwo()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
    }

    public void activateThree()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
    }

    public void activateFour()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = true; 
    }

    public void deactivateAll()
    {
        gameObject.transform.Find("Selector_1_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_2_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_3_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selector_4_big").gameObject.GetComponent<MeshRenderer>().enabled = false;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
