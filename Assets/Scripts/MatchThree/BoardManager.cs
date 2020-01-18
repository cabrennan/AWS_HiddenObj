/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;
	public List<Sprite> characters = new List<Sprite>();

    //public List<GameObject> prefabs = new List<GameObject>();


    public GameObject tile;
	public int xSize, ySize;

	private GameObject[,] tiles;

	public bool IsShifting { get; set; }

	void Start () {
		instance = GetComponent<BoardManager>();
        // GetCharacters;

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void GetCharacters(List<GameObject> prefabs)
    {
       
    }

	private void CreateBoard (float xOffset, float yOffset) {
		tiles = new GameObject[xSize, ySize];
        
        float startX = transform.position.x;
		float startY = transform.position.y;

        Sprite[] prevLeft = new Sprite[ySize];
        Sprite prevBelow = null;


		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
				tiles[x, y] = newTile;
                newTile.transform.parent = transform;

                List<Sprite> possibleChar= new List<Sprite>();

                possibleChar.AddRange(characters);

                possibleChar.Remove(prevLeft[y]);
                possibleChar.Remove(prevBelow);
                Sprite newSprite = possibleChar[Random.Range(0, possibleChar.Count)];
                
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
                prevLeft[y] = newSprite;
                prevBelow = newSprite;

			}
        }
    }

    public IEnumerator FindNullTiles()
    {
        for (int x=0; x<xSize; x++)
        {
            for(int y=0; y<ySize; y++)
            {
                Debug.Log("X is: " + x + " Y is: " + y);
                if(tiles[x,y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown(x, y));
                    break;
                }
            }
        }

        for(int x=0; x<xSize; x++)
        {
            for(int y=0; y<ySize; y++)
            {
                tiles[x,y].GetComponent<Tile>().ClearAllMatches();
            }
        }
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = 0.03f)
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for ( int y = yStart; y < ySize; y++)
        {
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            {
                nullCount++;
            }
            renders.Add(render);
        }

        for ( int i=0; i<nullCount; i++)
        {
            yield return new WaitForSeconds(shiftDelay);
            for ( int k=0; k<renders.Count-1; k++)
            {
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
               
            }
        }
        IsShifting = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleChars = new List<Sprite>();
         possibleChars.AddRange(characters);

        if(x >0)
        {
            possibleChars.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if(x < xSize -1)
        {
            possibleChars.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y >0)
        {
            possibleChars.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleChars[Random.Range(0, possibleChars.Count)];
    }


}
