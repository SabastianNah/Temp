using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControls : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }
}
