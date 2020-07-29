using NWH.VehiclePhysics2;
using UnityEngine;
using UnityEngine.UI;

public class HealthColorAnimation : MonoBehaviour
{
	public DemoGUIController healthValue;
	public Image imageColor;

	public Color32 colorHealthHigh;
	public Color32 colorHealthMidHigh;
	public Color32 colorHealthMid;
	public Color32 colorHealthMidLow;
	public Color32 colorHealthLow;

	[Range(0, 1)]public float valueHealthHigh = .2f;
	[Range(0, 1)] public float valueHealthMidHigh = .4f;
	[Range(0, 1)] public float valueHealthMid = .6f;
	[Range(0, 1)] public float valueHealthMidLow = .8f;
	[Range(0, 1)] public float valueHealthLow = 1;

	private void Update()
	{
		float _health = healthValue.animationHealth;

		if (_health <= valueHealthHigh)
		{
			imageColor.color = colorHealthHigh;
		}
		else if (_health >= valueHealthHigh && _health <= valueHealthMidHigh)
		{
			imageColor.color = colorHealthMidHigh;
		}
		else if (_health >= valueHealthMidHigh && _health <= valueHealthMid)
		{
			imageColor.color = colorHealthMid;
		}
		else if (_health >= valueHealthMid && _health <= valueHealthMidLow)
		{
			imageColor.color = colorHealthMidLow;
		}
		else if(_health >= valueHealthMidLow && _health <= valueHealthLow)
		{
			imageColor.color = colorHealthLow;
		}
		
	}
}
