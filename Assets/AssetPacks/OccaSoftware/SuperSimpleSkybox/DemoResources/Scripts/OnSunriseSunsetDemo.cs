using OccaSoftware.SuperSimpleSkybox.Runtime;
using UnityEngine;

namespace OccaSoftware.SuperSimpleSkybox.Demo
{
	[AddComponentMenu("SuperSimpleSkybox/Sunrise and Sunset Callbacks Demo")]
	public class OnSunriseSunsetDemo : MonoBehaviour
	{
		[SerializeField] Material skyboxMaterial;

		private Sun sun;

		private void OnEnable()
		{
			sun = FindObjectOfType<Sun>();
			if(sun != null)
			{
				sun.OnRise += Sunrise;
				sun.OnSet += Sunset;
			}
		}

		private void OnDisable()
		{
			if(sun != null)
			{
				sun.OnRise -= Sunrise;
				sun.OnSet -= Sunset;
			}
		}

		private void Sunrise()
		{
			skyboxMaterial.SetColor("_CloudColorDay", Random.ColorHSV());
			Debug.Log("Sunrise Event Triggered");
		}

		private void Sunset()
		{
			skyboxMaterial.SetColor("_CloudColorNight", Random.ColorHSV());
			Debug.Log("Sunset Event Triggered");
		}
	}
}