using UnityEngine;
using System.Collections;
using System;

public class PAnim {
	
	public static IEnumerator Animation (float duration, Action<float> animation, Action completion = null)
	{
		return Animation(duration, Linear, animation, completion: completion);
	}
	
	public static IEnumerator Animation (float duration, Func<float, float> timeCurve, Action<float> animation, Action completion = null)
	{
		float timer = 0;
		while (timer < duration) {
			float t = timeCurve(timer/duration);
			animation(t);
			timer += Time.deltaTime;
			yield return null;
		}
		animation(1);
		// Don't call it if it's not null
		if (completion != null) completion();
	}
	
	public static float Linear(float t)
	{
		return t;
	}
	
	// Ease In/Out/InOut functions Referencing http://gizma.com/easing/#cub3
	public static float EaseInOut(float t)
	{
		t *= 2;
		if (t < 1) return (t*t*t)/2.0f;
		t -= 2;
		return (t*t*t + 2)/2.0f;
	}
	
	public static float EaseIn(float t)
	{
		return t*t*t;
	}
	
	public static float EaseOut(float t)
	{
		t--;
		return t*t*t + 1;
	}
	
	public static Func<float, float> ElasticWithDamping(float zeta)
	{
		zeta = Mathf.Clamp01(zeta);
		return (float t) => {
			float z = zeta;
			if (t > zeta)
				z += Mathf.Pow(t-zeta, 4);
			float omega = Mathf.PI * Mathf.Sqrt(1 - z*z);
			return 1 - (Mathf.Exp(-t * 10 * z) * Mathf.Cos(omega * t * 10));
		};
	}
	
	public static float Elastic(float t)
	{
		float zeta = 0.5f;
		if (t > zeta)
			zeta += Mathf.Pow(t-zeta, 4);
		float omega = Mathf.PI * Mathf.Sqrt(1 - zeta*zeta);
		return 1 - (Mathf.Exp(-t * 10 * zeta) * Mathf.Cos(omega * t * 10));
	}
}
