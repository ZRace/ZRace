using Invector;
using NWH.VehiclePhysics2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDamagaZombiesCar : MonoBehaviour
{
	public VehicleController _nearestVehicle;
	public float velocity;
	public vObjectDamage activateScript;


	public ActivateDamagaZombiesCar(VehicleController nearestVehicle)
	{
		this._nearestVehicle = nearestVehicle;
	}
	private void Update()
	{
		velocity = _nearestVehicle.Speed;
	}
}
