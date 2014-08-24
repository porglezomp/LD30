using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public void Hide()
	{
		Vector3 start = transform.position;
		Vector3 end = transform.position - Vector3.up * 7.5f;
		StartCoroutine(PAnim.Animation(2, PAnim.ElasticWithDamping(0.7f), (t) => {
			transform.position = Extrap.Lerp(start, end, t);
			Universe.instance.StartGame ();
		}));
	}

	public void Show()
	{
		Vector3 start = transform.position;
		Vector3 end = transform.position + Vector3.up * 7.5f;
		StartCoroutine(PAnim.Animation(2, PAnim.ElasticWithDamping(0.7f), (t) => {
			transform.position = Extrap.Lerp(start, end, t);
		}));
	}
}
