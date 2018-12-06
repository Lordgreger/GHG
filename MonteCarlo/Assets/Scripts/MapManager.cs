using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public GameObject player;
    public GameObject tile;
    public GameObject goal;
    public GameObject ghost;

    private List<GameObject> board = new List<GameObject>();

    void ClearBoard()
    {
        foreach (GameObject g in board)
        {
            Destroy(g);
        }
        board.Clear();
    }

    public void ArrangeMap(char[] map)
    {

        int y = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (i % 8 == 0)
            {
                y--;
            }

            GameObject newTile;
            switch (map[i])
            {
                case '@':
                    newTile = Instantiate(player, new Vector2(i % 8, y), Quaternion.identity);
                    break;
                case 'G':
                    newTile = Instantiate(goal, new Vector2(i % 8, y), Quaternion.identity);
                    break;
                case '#':
                    newTile = Instantiate(ghost, new Vector2(i % 8, y), Quaternion.identity);
                    break;
                default:
                    newTile = Instantiate(tile, new Vector2(i % 8, y), Quaternion.identity);
                    break;
            }
            newTile.transform.SetParent(this.transform);
            board.Add(newTile);
        }
    }
}
