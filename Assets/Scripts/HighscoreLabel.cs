using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreLabel : MonoBehaviour {

	public Text score;
	public Text player;
	Color _color;
	public Color color {
		get { return _color; }
		set {
			_color = value;
			score.color = color;
			player.color = color;
		}
	}
	
	public void Setup (int score, string player, Color color)
	{
		this.color = color;
		this.score.text = score.ToString();
		this.player.text = player;
	}
}
