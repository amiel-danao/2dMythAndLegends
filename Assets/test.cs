using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Dictionary<string, bool> test_dict;
    // Start is called before the first frame update
    void Start()
    {
        test_dict = new Dictionary<string, bool>();
        test_dict.Add("AARON", false);
        //Debug.Log(test_dict.ContainsKey("AARON"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
