using UnityEngine;
using System.Collections;

public class TileSpawner : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject nodePrefab;
	public GameObject canvas;
	public static TileSpawner instance;

	int combo = 0;
	public const int w = 8;
	public const int h = 8;
	const int tileStyles = 5;
	public int[] stacks = new int[w];
	Tile[,] tiles = new Tile[w, h];

	// Use this for initialization
	void Start ()
	{
		instance = this;
		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				AddTileAtPosition(x, y);
			}
		}
		DetectBreaks();
		DoBreaks();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void AddTileAtPosition(int x, int y, int? fallIn = null) {
		Vector3 pos;
		if (fallIn != null) {
//			stacks[x]++;
			pos = TileToWorld(x, y + 5);
		} else {
			pos = TileToWorld(x, y);
		}

		GameObject t = (GameObject)GameObject.Instantiate(tilePrefab);
		t.transform.position = pos;
		t.transform.parent = canvas.transform;
		t.name = "Tile";
		Tile tile = t.GetComponent<Tile>();
		tile.number = (int)Random.Range(0, tileStyles)+1;
		bool xbroken = true; bool ybroken = true;
		if (fallIn != null) {
			tile.Fall(y);
		} else {
			while (xbroken || ybroken){
				if (x > 1) {
					if (tiles[x-2, y].number == tile.number &&
					    tiles[x-1, y].number == tile.number) {
						xbroken = true;
						tile.number = (int)Random.Range(0, tileStyles)+1;
					} else xbroken = false;
				} else xbroken = false;
				if (y > 1) {
					if (tiles[x, y-2].number == tile.number &&
					    tiles[x, y-1].number == tile.number) {
						ybroken = true;
						tile.number = (int)Random.Range(0, tileStyles)+1;
					} else ybroken = false;
				} else ybroken = false;
			}
		}
		tiles[x, y] = tile;
	}

	public bool DetectBreaks ()
	{
		int nextBreakId = 0;
		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				if (tiles[x, y] == null) continue;
				int number = tiles[x, y].number;
				int? break_id = tiles[x, y].break_id;

				Tile[] toAdd = new Tile[w];
				int consecutive = 1;
				toAdd[0] = tiles[x, y];

				// Match horizontal runs
				for (int i = x+1; i < w; i++) {
					if (tiles[i, y] == null) break;
					if (break_id == tiles[i, y].break_id && break_id != null) {
						break; // We don't want to check the same stuff multiple times
					}
					if (tiles[i, y].number == number) {
						if (tiles[i, y].animating) { // If one of the ones is animating, cancel detection
							consecutive = 0;
							break;
						}
						toAdd[consecutive] = tiles[i, y];
						consecutive++;
					} else break; // The run has ended
				} // Match horizontal

				// If there were enough, add them to a break
				if (consecutive >= 3) {
					break_id = break_id ?? nextBreakId++;
//					print("Horizontal break of size " + consecutive + " with break ID "
//					      + break_id + ", starting at " + x + "" + y + ". " +
//					      "Break is equal to " + number + ".");
					for (int i = 0; i < consecutive; i++) {
						toAdd[i].break_id = break_id;
					}
				}

				toAdd = new Tile[h];
				toAdd[0] = tiles[x, y];
				consecutive = 1;
				for (int j = y+1; j < h; j++) {
					if (tiles[x, j] == null) break;
					if (break_id == tiles[x, j].break_id && break_id != null) {
						break; // We don't want to check the same stuff multiple times
					}
					if (tiles[x, j].number == number) {
						if (tiles[x, j].animating) { // If one of the ones is animating, cancel detection
							consecutive = 0;
							break;
						}
						toAdd[consecutive] = tiles[x, j];
						consecutive++;
					} else break; // The run has ended
				} // Match vertical

				// If there were enough matches, add them to a break
				if (consecutive >= 3) {
					break_id = break_id ?? nextBreakId++;
//					print("Vertical break of size "+ consecutive + " with break ID "
//					      + break_id + ", starting at " + x + "" + y + ". " +
//					      "Break is equal to " + number + ".");
					for (int i = 0; i < consecutive; i++) {
						toAdd[i].break_id = break_id;
					}
				}
			}
		}
		// If there's a break, do it
		if (nextBreakId > 0) {
			DoBreaks();
			return true;
		}
		return false;
	}

	public void DoBreaks()
	{
		int breakids = 0;
		int breaks=0;
		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				Tile tile = tiles[x, y];
				if (tile == null || tile.animating) continue;
				if (tile.break_id != null) {
					breaks++;
					if (tile.break_id > breakids) breakids = (int)tile.break_id;
					tile.Shatter();
					tiles[x, y] = null;
				}
			}
		}
		if (breaks > 0) {
			if (combo > 0) print(breaks + " breaks. COMBO " + combo);
			else print(breaks + " breaks, " + breakids + " unique.");
			Universe.instance.AddScore(breaks*breaks * (combo+1));
			combo++;
		}
		TileGravity();
	}

	void TileGravity()
	{
		int? open;
		for (int x = 0; x < w; x++) {
			open = null;
			for (int y = 0; y < h; y++) {
				if (tiles[x, y] == null && open == null) {
					open = y;
				} else if (open != null && tiles[x, y] != null) {
					tiles[x, y].Fall((int)open);
					tiles[x, (int)open] = tiles[x, y];
					tiles[x, y] = null;
					open++;
				}
			}
			if (open != null) {
				for (int i = (int)open; i < h; i++) {
					AddTileAtPosition(x, i, fallIn: (h-open));
				}
			}
		}
		if (DetectBreaks()) {
			DoBreaks();
		}
	}

	public bool Swap(int x1, int y1, int x2, int y2)
	{
		if (x1 != x2 || y1 != y2) {
			combo = 0;
		}
		Tile temp = tiles[x1, y1];
		tiles[x1, y1] = tiles[x2, y2];
		tiles[x2, y2] = temp;
		if (DetectBreaks()) {
			// Put them in their proper places
			tiles[x1, y1].Goto(x1, y1);
			tiles[x2, y2].Goto(x2, y2);
			DoBreaks();
			return true;
		} else { // Swap it back if it didn't do anything
			temp = tiles[x1, y1];
			tiles[x1, y1] = tiles[x2, y2];
			tiles[x2, y2] = temp;
			return false;
		}
	}
	
	public static Vector2 TileToWorld(int x, int y)
	{
		return new Vector2((x-3.5f) * 0.95f, (y-3.5f) * 0.95f);
	}

	// Algebra time!
	// x(t) = (t-3.5) * 0.95
	// =>
	// x = (y-3.5) * 0.95
	// x / 0.95 = y - 3.5
	// x/0.95 + 3.5 = y

	public static void WorldToTile(Vector2 pos, out int x, out int y)
	{
		x = Mathf.RoundToInt(pos.x/0.95f + 3.5f);
		y = Mathf.RoundToInt(pos.y/0.95f + 3.5f);
	}

	public void SetTile(int x, int y, Tile t) {
		print("Add " + x + " " + y);
		if (x > 0 && x < w && y > 0 && y < h) {
			print("Yes added");
			tiles[x, y] = t;
		}
	}
}
