using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void NewGame()
	{
		Vector3 start = transform.position;
		Vector3 end = transform.position - Vector3.up * 7.5f;
		StartCoroutine(PAnim.Animation(2, PAnim.Elastic, (t) => {
			transform.position = Extrap.Lerp(start, end, t);
		}));
	}
}
