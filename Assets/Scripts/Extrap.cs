using UnityEngine;
using System.Collections;

public class Extrap {
	
	public static Quaternion Slerp(Quaternion q1, Quaternion q2, float t)
	{
		float angle;
		Vector3 axis;
		
		// FromToRotation is for vectors, so we use Quaternion.RotateTowards
		// Use 360º RotateTowards so that it will always be the full rotation
		Quaternion.RotateTowards(Quaternion.identity, Quaternion.Inverse(q1)*q2, 360).ToAngleAxis(out angle, out axis);
		angle *= t;
		return Quaternion.AngleAxis(angle, axis)*q1;
	}
	
	public static Vector3 Lerp(Vector3 v1, Vector3 v2, float t)
	{
		Vector3 d = v2 - v1;
		return v1 + d*t;
	}
	
	public static float Lerp(float x, float y, float t)
	{
		return x + (y - x)*t;
	}
}