using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(3.0f);

        this.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    private void OnEnable()
    {
        StartCoroutine("ActiveFalse");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
