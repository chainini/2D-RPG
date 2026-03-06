using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    private void Update()
    {
        if(Time.timeScale==0)
            Time.timeScale = 1;
    }
}
