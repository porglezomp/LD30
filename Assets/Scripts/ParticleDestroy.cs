using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour 
{	
	ParticleSystem ps;

	void Start ()
	{
		ps = gameObject.particleSystem;
		if (ps == null) {
			ps = gameObject.GetComponentInChildren<ParticleSystem>();
		}
	}

	void Update() 
	{
		if (ps) {
			if (!ps.IsAlive()) {
				Destroy(gameObject);
			}
		}
	}
}