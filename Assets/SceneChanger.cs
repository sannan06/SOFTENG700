using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public Dropdown dropdown;
    private string currentDropDownLevel;

    void Start()
    {
        currentDropDownLevel = dropdown.options[dropdown.value].text;
        Debug.Log(currentDropDownLevel);

    }
    public void handleDropDownChange(Dropdown change) {
        currentDropDownLevel = change.options[change.value].text;
        Debug.Log(currentDropDownLevel);
    }

    public void handleButtonClick() {
        SceneManager.LoadScene(currentDropDownLevel);
    }
}
