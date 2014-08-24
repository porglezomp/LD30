using UnityEngine;
using System.Collections;

public class AssetManager : MonoBehaviour {

	public AudioClip[] clinks;
	public AudioClip[] bangs;
	static AudioClip[] _clinks;
	static AudioClip[] _bangs;

	// Use this for initialization
	void Start () {
		_clinks = clinks;
		_bangs = bangs;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static AudioClip GetClink()
	{
		int id = Random.Range(0, _clinks.Length);
		return _clinks[id];
	}

	public static AudioClip GetBang()
	{
		int id = Random.Range (0, _bangs.Length);
		return _bangs[id];
	}
}
