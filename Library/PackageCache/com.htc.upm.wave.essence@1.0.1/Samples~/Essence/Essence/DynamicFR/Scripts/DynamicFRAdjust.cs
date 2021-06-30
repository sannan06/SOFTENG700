using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DynamicFR))]
public class DynamicFRAdjust : MonoBehaviour
{
	DynamicFR dynamicFR;

	public Slider sliderAV;
	public Slider sliderVF;
	public Slider sliderVB;
	public Slider sliderVZ;
	public Slider sliderFR;
	public Slider sliderRS;
	public Slider sliderVS;

	private void OnEnable()
	{
		dynamicFR = GetComponent<DynamicFR>();
	}

	void Start()
    {
		sliderAV.maxValue = 1;
		sliderAV.minValue = 0;
		sliderVF.maxValue = 1;
		sliderVF.minValue = 0;
		sliderVB.maxValue = 1;
		sliderVB.minValue = 0;
		sliderVZ.maxValue = 1;
		sliderVZ.minValue = 0;

		sliderFR.maxValue = 3;
		sliderFR.minValue = 0;
		sliderRS.maxValue = 3;
		sliderRS.minValue = 0;
		sliderVS.maxValue = 3;
		sliderVS.minValue = 0;

		sliderAV.value = dynamicFR.AVWeight;
		sliderVF.value = dynamicFR.VFWeight;
		sliderVB.value = dynamicFR.VBWeight;
		sliderVZ.value = dynamicFR.VZWeight;
		sliderFR.value = dynamicFR.FRWeight;
		sliderRS.value = dynamicFR.RSWeight;
		sliderVS.value = dynamicFR.VSWeight;
	}

	public void OnValueChanged(Slider slider)
	{
		Debug.Log("Slider value = " + slider.value);

		if (slider == sliderAV)
			dynamicFR.AVWeight = slider.value;
		else if (slider == sliderVF)
			dynamicFR.VFWeight = slider.value;
		else if (slider == sliderVB)
			dynamicFR.VBWeight = slider.value;
		else if (slider == sliderVZ)
			dynamicFR.VZWeight = slider.value;
		else if (slider == sliderFR)
			dynamicFR.FRWeight = slider.value;
		else if (slider == sliderRS)
			dynamicFR.RSWeight = slider.value;
		else if (slider == sliderVS)
			dynamicFR.VSWeight = slider.value;

		Transform textTransform = slider.transform.parent.Find("WeightLabel");
		if (textTransform != null)
		{
			Text text = textTransform.GetComponent<Text>();
			text.text = slider.transform.parent.name + " weight=\n" + slider.value;
		}
	}
}
