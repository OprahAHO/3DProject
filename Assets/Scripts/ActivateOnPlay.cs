using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivateOnPlay : MonoBehaviour
{
  
    void Start()
    {
        GetComponent<TextMeshProUGUI>().enabled = true;
    }
}
    

