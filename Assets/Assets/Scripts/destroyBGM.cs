using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Object bgm = GameObject.Find("BGM");
        Destroy(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
