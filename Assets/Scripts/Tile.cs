using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tile : MonoBehaviour {

	public bool animating;
	int _number;
	public int number {
		get { return _number; }
		set { 
			_number = value;
			label.text = number.ToString();
		}
	}

	public int? break_id = null;

	public Text label;
	Vector3 grabOffset;
	Vector3 grabPoint;
	Vector3 startPoint;
	
	public GameObject particles;

	public void OnMouseOver ()
	{
		// Do some mouseover animation. Rotation jiggle?
	}

	public void OnMouseDown ()
	{
		startPoint = transform.position;
		grabPoint = InputWorldPos();
		grabOffset = startPoint - grabPoint;
	}

	public void OnMouseDrag ()
	{
		Vector2 delta = InputWorldPos() - grabPoint;
		if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
			delta = new Vector2(Mathf.Clamp(delta.x, -1, 1), 0);
		} else {
			delta = new Vector2(0, Mathf.Clamp(delta.y, -1, 1));
		}
		transform.position = new Vector3(startPoint.x + delta.x, 
		                                 startPoint.y + delta.y, 
		                                 -0.1f);

	}

	public void OnMouseUp ()
	{
		int my_x, my_y;
		int new_x, new_y;
		TileSpawner.WorldToTile(startPoint, out my_x, out my_y);
		TileSpawner.WorldToTile(transform.position, out new_x, out new_y);
		// If the swap fails, return to the original position
		if (new_x >= 0 && new_x < TileSpawner.w && new_y >= 0 && new_y < TileSpawner.h) {
			if (!TileSpawner.instance.Swap(my_x, my_y, new_x, new_y)) {
				Goto(my_x, my_y);
			}
		} else {
			Goto(my_x, my_y);
		}
	}

	Vector3 InputWorldPos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward*18.5f);
	}

	public void Goto(int x, int y)
	{
		Vector3 start = transform.position;
		Vector3 goal = (Vector3)TileSpawner.TileToWorld(x, y) + Vector3.forward * transform.position.z;
		Vector3 goal2 = new Vector3(goal.x, goal.y, 0);
		animating = true;
		StartCoroutine(PAnim.Animation(0.5f, PAnim.Elastic, (t) => {
			transform.position = Extrap.Lerp(start, goal, t);
		}, completion: () => {
			StartCoroutine(PAnim.Animation(0.3f, PAnim.Elastic, (t) => {
				transform.position = Extrap.Lerp(goal, goal2, t);
			}, completion: () => {
				animating = false;
				TileSpawner.instance.DetectBreaks();
			}));
		}));
	}

	public void Fall(int y)
	{
		int x, trash;
		TileSpawner.WorldToTile(transform.position, out x, out trash);
		Vector3 goal = TileSpawner.TileToWorld(x, y);
		animating = true;
		StartCoroutine(_fall(goal));
	}

	IEnumerator _fall(Vector3 goal) {
		float dy = 0;
		float cy = transform.position.y;
		while (transform.position.y > goal.y) {
			yield return null;
			transform.position = new Vector3(goal.x, cy);
			cy += dy;
			dy -= 1f * Time.deltaTime;
		}
		transform.position = goal;
		animating = false;
		int x, y;
		TileSpawner.WorldToTile(transform.position, out x, out y);
//		TileSpawner.instance.stacks[x]--;
		TileSpawner.instance.DetectBreaks();
	}

	public void Shatter()
	{
		Instantiate(particles, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
