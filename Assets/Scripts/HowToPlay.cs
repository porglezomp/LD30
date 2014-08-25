using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class HowToPlay : MonoBehaviour {

	public void Hide()
	{
		RectTransform trans = gameObject.GetComponent<RectTransform>();
		Vector3 start = trans.anchoredPosition;
		Vector3 end = trans.anchoredPosition - Vector2.up * 7.5f;
		StartCoroutine(PAnim.Animation(2, PAnim.ElasticWithDamping(0.7f), (t) => {
			trans.anchoredPosition = Extrap.Lerp(start, end, t);
		}));
	}
	
	public void Show()
	{
		RectTransform trans = gameObject.GetComponent<RectTransform>();
		Vector3 start = trans.anchoredPosition;
		Vector3 end = trans.anchoredPosition + Vector2.up * 7.5f;
		StartCoroutine(PAnim.Animation(2, PAnim.ElasticWithDamping(0.7f), (t) => {
			trans.anchoredPosition = Extrap.Lerp(start, end, t);
		}));
	}
}
