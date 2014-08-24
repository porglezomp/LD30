using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Universe : MonoBehaviour {
	public const int maxworlds = 2;
	public Color[] keyColor;
	public Color[] fillColor;
	public int[] scores = new int[maxworlds];
	public Light keyLight;
	public Light fillLight;
	public Transform background;
	public Transform spinner;
	public Text overworldScore;
	public Text underworldScore;
	public static Universe instance;

	public HighscoreFill whiteScoreboard;
	public HighscoreFill blueScoreboard;
	public HighscoreFill redScoreboard;

	float timer = 20;
	int _world;
	public int world {
		get { return _world; }
		set { 
			Color old_key = keyColor[_world];
			Color old_fill = fillColor[_world];

			_world = value;
			StartCoroutine(_lightLerp(keyLight, old_key, keyColor[_world]));
			StartCoroutine(_lightLerp(fillLight, old_fill, fillColor[_world]));
		}
	}

	public void Start ()
	{
		Universe.instance = this;

		HighScores();
	}

	public void Update ()
	{
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer += Random.Range(15, 25);
			world = (world+1)%maxworlds;
			_spin();
		}
	}

	public void AddScore (int score)
	{
		scores[world] += score;
		overworldScore.text = scores[0] + " Points";
		underworldScore.text = scores[1] + " Points";
	}

	IEnumerator _lightLerp (Light l, Color c1, Color c2)
	{
		float timer = 0;
		while (timer < 1) {
			timer += Time.deltaTime;
			l.color = Color.Lerp(c1, c2, timer);
			yield return null;
		}
	}
	
	void _spin ()
	{
		float start = _world > 0 ? 179 : 1;
		StartCoroutine(PAnim.Animation(5, PAnim.ElasticWithDamping(0.8f), (t) => {
			Quaternion angle = Quaternion.Euler(0, 0, 179*t + start);
			background.localRotation = angle;
			spinner.localRotation = angle;
		}, completion: () => {
			background.Rotate(0, 0, 1);
			spinner.Rotate(0, 0, 1);
		}));
	}

	const int highscoreDepth = 8;
	public void SaveScore (string scoreboard, int score, string name)
	{
		int[] scoreboardScores = new int[highscoreDepth];
		for (int i = 0; i < highscoreDepth; i++) {
			if (PlayerPrefs.HasKey(scoreboard+i)) {
				scoreboardScores[i] = PlayerPrefs.GetInt(scoreboard+i);
			} else {
				scoreboardScores[i] = 0;
			}
		}
		int location = highscoreDepth;
		for (int i = 0; i < highscoreDepth; i++) {
			if (score > scoreboardScores[i])
			{
				location = i;
				break;
			}
		}
		if (location != highscoreDepth) {
			for (int i = highscoreDepth-1; i > location; i--) {
				if (PlayerPrefs.HasKey(scoreboard+(i-1))) {
					PlayerPrefs.SetInt(scoreboard+i, PlayerPrefs.GetInt(scoreboard+(i-1)));
					PlayerPrefs.SetString(scoreboard+i+"p", PlayerPrefs.GetString(scoreboard+(i-1)+"p"));
				} else {
					PlayerPrefs.SetInt(scoreboard+i, 0);
					PlayerPrefs.SetString(scoreboard+i+"p", "");
				}
			}
			PlayerPrefs.SetInt(scoreboard+location, score);
			PlayerPrefs.SetString(scoreboard+location+"p", name);
		}
	}

	public void LoadHighscores (string scoreboard, out int[] scores, out string[] players)
	{
		scores = new int[highscoreDepth];
		players = new string[highscoreDepth];
		for (int i = 0; i < highscoreDepth; i++) {
			if (PlayerPrefs.HasKey(scoreboard+i)) scores[i] = PlayerPrefs.GetInt(scoreboard+i);
			else scores[i] = 0;
			if (PlayerPrefs.HasKey(scoreboard+i+"p")) players[i] = PlayerPrefs.GetString(scoreboard+i+"p");
			else players[i] = "";
		}
	}

	void HighScores()
	{
		whiteScoreboard.Fill();
		blueScoreboard.Fill();
		redScoreboard.Fill();
	}
}
