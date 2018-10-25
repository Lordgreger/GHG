using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioning : MonoBehaviour {

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    int[,] map;

    public int maxDepth = 0;

    // Use this for initialization
    void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateMap()
    {
        // Create a Block representing the entire map: starting at (0,0) and with the same width and height specified by the user
        print("Height, Width: " + height + "," + width);
        Block dungeon = new Block( 0, 0, width, height);

        if (useRandomSeed)
        {
            seed = System.DateTime.Now.ToString("h:mm:ss tt");
            print("Random seed: " + seed);
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        dungeon.Cut(maxDepth, pseudoRandom);

        map = dungeon.CreateMap(pseudoRandom);
        //Debug.Log("Map dim: " + map.GetLength(0) + " by " + map.GetLength(1));
    }

    void OnDrawGizmos()
    {
        if (map != null)
        {
            //Debug.Log("Map dim: " + map.GetLength(0) + " by " + map.GetLength(1));
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, new Vector3(0.8f, 0.1f, 0.8f));
                }
            }
        }
    }
}

/* In this file we also define a Block class, this is not a MonoBehaviour class (no ": MonoBehaviour" after the class name)
 * This means that it does not inherit all the Update/Start/etc. functions we got used to
 * We need to define all functions we need.
 * In this case we want this class to represent a block of space which will either be:
 *  1) split into two
 *  2) or contain a room
 * In case 1, we will save the two blocks in the "children" variable
 * In case 2, the "children" variable will be empty (null), and we'll have the inf about the room in "roomCoordinates" and "roomSizes"
 * 
 * You define a block by specifying where it starts (x,y coordinates) and how wide/tall it is
 * e.g: 
 *  Block myBlock = new Block(1, 2, 2, 3); //defines a block starting at (1,2), wide 2 and tall 3
 *  
 *  WHAT YOU HAVE TO IMPLEMENT:
 *  CutBlock function and CreateRoom function
 */
public class Block
{
    int blockX;
    int blockY;

    int blockWidth;
    int blockHeight;

    int minBlockWallSize = 4;
    int minRoomWallSize = 2;

    int roomX;
    int roomY;

    int roomWidth;
    int roomHeight;

    Block[] children = null;

    // Block class instatiation, use this to create a new Block at the specified coordinates and with the specified width and height
    public Block(int x, int y, int width, int height)
    {
        blockX = x;
        blockY = y;
        blockWidth = width;
        blockHeight = height;
    }

    /* This function is the one that deals with creating the tree of blocks recursively
     * You call it specifying how deep do you want the tree to go and a random number generator (so you always get the same result with the specified seed)
     * The function calls other Cut functions in the newly created blocks, until the specified end is reached
     */
    public void Cut(int maxDepth, System.Random random)
    {
        // if we need to cut more (maxDepth more than 0) then cut the block, otherwise this is a final block and let's create a room in it
        if (maxDepth > 0)
        {
            // CutBlock returns true if it successfully cut the block, or false if the block was too small to be cut
            if (CutBlock(random))
            {
                // if we have cut then cut the children as well
                children[0].Cut(maxDepth - 1, random);
                children[1].Cut(maxDepth - 1, random);
            }
            else
            {
                CreateRoom(random);
            }
        }
        else
        {
            CreateRoom(random);
        }
    }

    /*
     * This is where the logic for splitting a block in two should go.
     * The vanilla BSP should choose at random if to cut horizontally or vertically and then choose a random index at which to do the cutting
     * NOTE: use minBlockSize to be sure you don't get blocks with width/height of 0
     * NOTE: remember to save the newly defined Blocks in the "children" array 
     *  e.g:    Block b1 = new Block(...); //fill in the right values
     *          Block b2 = new Block(...);
     *          children = new Block[] {b1, b2};
     */
    private bool CutBlock(System.Random random)
    {
        Debug.Log("Send a block to the cutting board!");

        bool horizontalCut = random.Next(0,2) == 1; //if 0 vertical, if 1 horizontal

        Debug.Log("Cutting dir is horizontal = " + horizontalCut);

        if (horizontalCut)
        {
            //Debug.Log(minBlockWallSize * 2 + " < " + blockHeight);
            if ((minBlockWallSize * 2) > blockHeight) { // Catch for too small blocks
                //Debug.Log("Too small to be cut!");
                return false;
            }
                

            int cutPlacement = random.Next(minBlockWallSize, blockHeight - minBlockWallSize);

            Block b1 = new Block(blockX, blockY, blockWidth, cutPlacement);
            Block b2 = new Block(blockX, blockY + cutPlacement + 1, blockWidth, blockHeight - cutPlacement);
            children = new Block[] { b1, b2 };

            return true;
        }
        else 
        {
            //Debug.Log(minBlockWallSize * 2 + " < " + blockWidth);
            if ((minBlockWallSize * 2) > blockWidth) { // Catch for too small blocks
                //Debug.Log("Too small to be cut!");
                return false;
            }

            int cutPlacement = random.Next(minBlockWallSize, blockWidth - minBlockWallSize);

            Block b1 = new Block(blockX, blockY, cutPlacement, blockHeight);
            Block b2 = new Block(blockX + cutPlacement + 1, blockY, blockWidth - cutPlacement, blockHeight);
            children = new Block[] { b1, b2 };

            return true;
        }
    }

    /* This function should create a room in the current block
     * You should do so by setting the roomSizes and roomCoordinates variables
     * NOTE use the minRoomSize to avoid 0-sized rooms
     * IMPORTANT: The roomCoordinates are LOCAL, they refer to the starting point of the block as (0,0)
     */
    private void CreateRoom(System.Random random)
    {
        Debug.Log("Creating room in block of size: " + blockWidth + "," + blockHeight);
        roomWidth = random.Next(minRoomWallSize, blockWidth); // ADD HERE the size of room on x dimension
        roomHeight = random.Next(minRoomWallSize, blockHeight); ; // ADD HERE the size of room on y dimension
        Debug.Log("Room size: " + roomWidth + "," + roomHeight);

        roomX = random.Next(1, (blockWidth - roomWidth)); // ADD HERE the offset on the x axis
        roomY = random.Next(1, (blockHeight - roomHeight)); // ADD HERE offset on the y axis

        //Debug.Log("room of size " + roomWidth + "," + roomHeight + " at (" + (blockX + roomX) + "," + (blockY + roomY) + ")");
    }

    /* This function creates a map of all the rooms by recursively asking each block in the tree to create a map and then collating them together
     */
    public int[,] CreateMap(System.Random random) {
        // Create a bi-dimensional array "map" with the block size
        int[,] map = new int[blockWidth, blockHeight];
        //Debug.Log("Map dim: " + map.GetLength(0) + " by " + map.GetLength(1));

        // If the block has no children that means it is a final block  (with a room) so create the map corresponding to it.
        if (children == null)
        {
            for (int x = 0; x < blockWidth; x++)
            {
                for (int y = 0; y < blockHeight; y++)
                {
                    if ((x < roomX || y < roomY) ||
                        (x >= roomX + roomWidth || y >= roomY + roomHeight))
                    {
                        map[x, y] = 0;
                    }
                    else
                    {
                        map[x, y] = 1;
                    }
                }
            }
        }
        else // The block has children, so we need to calculate the maps for these and then put them together
        {
            int[,] map1 = children[0].CreateMap(random);
            int[,] map2 = children[1].CreateMap(random);

            if (children[0].blockX < children[1].blockX)
            {
                //if the blocks are separated vertically
                map = new int[map1.GetLength(0) + map2.GetLength(0), map1.GetLength(1)];

                for (int x = 0; x < map1.GetLength(0); x++){
                    for (int y = 0; y < map1.GetLength(1); y++){
                            map[x, y] = map1[x, y];
                    }
                }

                for (int x = 0; x < map2.GetLength(0); x++) {
                    for (int y = 0; y < map2.GetLength(1); y++) {
                        map[x + map1.GetLength(0), y] = map2[x, y];
                    }
                }

                //CreateCorridor(false, random, map);
            }
            else
            {
                //if the blocks are separated horizontally
                map = new int[map1.GetLength(0), map1.GetLength(1) + map2.GetLength(1)];

                for (int x = 0; x < map.GetLength(0); x++){
                    for (int y = 0; y < map1.GetLength(1); y++){
                        map[x, y] = map1[x, y];
                    }
                    for (int y = 0; y < map2.GetLength(1); y++) {
                        map[x, y + map1.GetLength(1)] = map2[x, y];
                    }
                }
                CreateCorridor(true, random, map);
            }
        }
        return map;
    }

    /* This function should add a corridor between connected rooms (siblings)
     * NOTE: it will likely need some arguments to be passed to be able to properly make a corridor
     */
    private void CreateCorridor(bool horizontal, System.Random random, int[,] map)
    {
        if (horizontal) {
            // Setup some information for easier use
            int room0start = children[0].roomX;
            int room0end = room0start + children[0].roomWidth;

            int room1start = children[1].roomX;
            int room1end = room1start + children[1].roomWidth;

            // Catch for no overlap
            if (room0end < room1start || room1end < room0start) {
                Debug.Log("No overlap!");
                return;
            }

            Debug.Log("roomX: " + children[0].roomX + "," + children[1].roomX);


            // Find overlap start and end
            int overlapStart;
            Debug.Log("starts: " + room0start + "," + room1start);
            if (room0start < room1start) {
                overlapStart = room1start;
            }
            else {
                overlapStart = room0start;
            }

            int overlapEnd;
            Debug.Log("ends: " + room0end + "," + room1end);
            if (room0end < room1end) {
                overlapEnd = room1end;
            }
            else {
                overlapEnd = room0end;
            }

            // Catch for top
            if (overlapStart == 0 && overlapEnd == 0) {
                Debug.Log("Weird overlap!");
                return;
            }

            // Generate corridor params
            Debug.Log("Overlap: " + overlapStart + "," + overlapEnd);
            int corridorX = random.Next(overlapStart, overlapEnd);
            Debug.Log("Created corridor at x: " + corridorX);

            // Paint corridor on map
            int yStart;
            //Debug.Log("blockX's: " + children[0].blockX + "," + children[1].blockX);
            if (children[0].blockY < children[1].blockY) {
                yStart = children[0].roomY + children[0].roomHeight;
            }
            else {
                yStart = children[1].roomY + children[1].roomHeight;
            }
            int yEnd = yStart + blockHeight - children[0].roomHeight - children[1].roomHeight - children[0].roomY - children[1].roomY;

            Debug.Log("Corridor lenght: " + (yEnd - yStart));
            Debug.Log("from, to: " + yStart + "," + yEnd);
            Debug.Log("map size: " + map.GetLength(0) + "," + map.GetLength(1));

            for (int y = yStart; y <= yEnd; y++) {
                //Debug.Log("Drawing at: " + corridorX + "," + y);
                map[corridorX, y] = 1;
            }
        }
        else {
            // Setup some information for easier use
            int room0start = children[0].roomY;
            int room0end = room0start + children[0].roomHeight;

            int room1start = children[1].roomY;
            int room1end = room1start + children[1].roomHeight;

            // Catch for no overlap
            if (room0end < room1start || room1end < room0start) {
                Debug.Log("No overlap!");
                return;
            }

            Debug.Log("roomY: " + children[0].roomY + "," + children[1].roomY);

            // Find overlap start and end
            int overlapStart;
            Debug.Log("starts: " + room0start + "," + room1start);
            if (room0start < room1start) {
                overlapStart = room1start;
            }
            else {
                overlapStart = room0start;
            }

            int overlapEnd;
            Debug.Log("ends: " + room0end + "," + room1end);
            if (room0end > room1end) {
                overlapEnd = room1end;
            }
            else {
                overlapEnd = room0end;
            }

            // Catch for top
            if (overlapStart == 0 && overlapEnd == 0) {
                Debug.Log("Weird overlap hori!");
                return;
            }

            // Generate corridor params
            Debug.Log("Overlap: " + overlapStart + "," + overlapEnd);
            int corridorY = random.Next(overlapStart, overlapEnd);
            Debug.Log("Created corridor at y: " + corridorY);

            // Paint corridor on map
            int xStart;
            //Debug.Log("blockX's: " + children[0].blockX + "," + children[1].blockX);
            if (children[0].blockX < children[1].blockX) {
                xStart = children[0].roomX + children[0].roomWidth;
            }
            else {
                xStart = children[1].roomX + children[1].roomWidth;
            }
            int xEnd = xStart + blockWidth - children[0].roomWidth - children[1].roomWidth - children[0].roomX - children[1].roomX;

            Debug.Log("from, to: " + xStart + "," + xEnd);
            Debug.Log("map size: " + map.GetLength(0) + "," + map.GetLength(1));
            Debug.Log("Corridor lenght: " + (xEnd - xStart));

            for (int x = xStart; x < xEnd; x++) {
                Debug.Log("Drawing at: " + x + "," + corridorY);
                map[x, corridorY] = 1;
            }
        }
    }
}
