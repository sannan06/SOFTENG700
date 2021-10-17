using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
    Class SceneChanger is used to siwtch between different activities dpending on dropdown.
*/
public class SceneChanger : MonoBehaviour
{
    public Dropdown dropdown;
    private string currentDropDownLevel;

    void Start()
    {
        currentDropDownLevel = dropdown.options[dropdown.value].text;
        Debug.Log(currentDropDownLevel);

    }

    /**
        Update level based on drop down.
    */
    public void handleDropDownChange(Dropdown change) {
        currentDropDownLevel = change.options[change.value].text;
        Debug.Log(currentDropDownLevel);
    }


    /**
        Switch to scene on button click
    */
    public void handleButtonClick() {
        SceneManager.LoadScene(currentDropDownLevel);
    }
}
