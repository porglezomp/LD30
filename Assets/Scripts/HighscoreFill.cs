using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreFill : MonoBehaviour {

	public string boardName;
	public Color boardColor;
	public GameObject scoreCard;
	int[] scores;
	string[] players;
	GameObject labels;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Fill ()
	{
		Universe.instance.LoadHighscores(boardName, out scores, out players);
		int len = scores.Length;
		for (int i = 0; i < len; i++) {
			GameObject obj = (GameObject)GameObject.Instantiate(scoreCard);
			RectTransform t = (RectTransform)obj.transform;
			t.parent = transform;
			t.pivot = new Vector2(0.5f, 1);
			t.localPosition = Vector3.zero;
			t.anchoredPosition = new Vector2(0, -35 * i);
			t.localScale = Vector3.one;
			HighscoreLabel lbl = obj.GetComponent<HighscoreLabel>();
			lbl.Setup(scores[i], players[i], boardColor);
		}
	}
}
