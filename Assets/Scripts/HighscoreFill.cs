using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreFill : MonoBehaviour {

	public string boardName;
	public Color boardColor;
	public GameObject scoreCard;
	int[] scores;
	string[] players;
	GameObject[] labels;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Fill ()
	{
		if (scores == null) {
			scores = new int[8];
		}
		int len = scores.Length;
		if (labels == null || labels.Length < len) {
			if (labels != null) {
				for (int i = 0; i < labels.Length; i++) {
					Destroy(labels[i]);
				}
			}
			labels = new GameObject[len];
			Universe.instance.LoadHighscores(boardName, out scores, out players);
			for (int i = 0; i < len; i++) {
				GameObject obj = (GameObject)GameObject.Instantiate(scoreCard);
				labels[i] = obj;
				RectTransform t = (RectTransform)obj.transform;
				t.parent = transform;
				t.pivot = new Vector2(0.5f, 1);
				t.localPosition = Vector3.zero;
				t.anchoredPosition = new Vector2(0, -39 * i);
				t.localScale = Vector3.one;
				HighscoreLabel lbl = obj.GetComponent<HighscoreLabel>();
				lbl.Setup(scores[i], players[i], boardColor);
			}
		} else {
			Universe.instance.LoadHighscores(boardName, out scores, out players);
			for (int i = 0; i < len; i++) {
				HighscoreLabel lbl = labels[i].GetComponent<HighscoreLabel>();
				lbl.Setup(scores[i], players[i], boardColor);
			}
		}
	}
}
