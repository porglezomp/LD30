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

	float timer = 2;
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
	}

	public void Update ()
	{
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer += Random.Range(12, 18);
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
}
