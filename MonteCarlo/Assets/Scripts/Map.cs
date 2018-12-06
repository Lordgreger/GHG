using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

    // --- variables
    protected const int mapsize = 8;

    protected char[] startingmap ={ '@',' ',' ',' ',' ',' ',' ',' ',
                                         ' ',' ',' ',' ',' ',' ',' ',' ',
                                         ' ',' ',' ',' ',' ',' ',' ',' ',
                                         ' ',' ',' ',' ',' ',' ',' ',' '
                                         ,' ',' ',' ','#',' ',' ','G',' '
                                         ,' ',' ',' ',' ',' ',' ',' ',' '
                                         ,' ',' ',' ',' ',' ',' ',' ',' '
                                         ,' ',' ',' ',' ',' ',' ',' ',' '
    };
    public char[] map;

    // --- movement constants
    public const int UP = 0;
    public const int RIGHT = 1;
    public const int DOWN = 2;
    public const int LEFT = 3;

    public char[] GetMap()
    {
        return (char[])map.Clone();
    }

    /**
     * Assign reward 1 if won, 0 if lost
     * @param st
     * @return
     */
    public float GetReward(char[] st)
    {
        if (IsGoalReached(st))
            return 1;
        if (IsAvatarDead(st))
        {
            return 0;
        }
        return 0;
    }

    public void ResetMaze()
    {
        map = (char[])startingmap.Clone();
    }

    /**
     * Returns the map state which results from an initial map state after an
     * action is applied. In case the action is invalid, the returned map is the
     * same as the initial one (no move).
     * @param action taken by the avatar ('@')
     * @param current map before the action is taken
     * @return resulting map after the action is taken
     */
    public char[] GetNextState(int action, char[] map)
    {
        char[] nextMap = (char[])map.Clone();
        // get location of '@'
        int avatarIndex = GetAvatarIndex(map);
        if (avatarIndex == -1)
        {
            return nextMap; // no effect
        }
        int nextAvatarIndex = GetNextAvatarIndex(action, avatarIndex);
        if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length)
        {
            if (nextMap[nextAvatarIndex] != '#')
            {
                // change the map
                nextMap[avatarIndex] = ' ';
                nextMap[nextAvatarIndex] = '@';
            }
        }
        return nextMap;
    }

    public char[] GetNextState(int action)
    {
        char[] nextMap = (char[])map.Clone();
        // get location of '@'
        int avatarIndex = GetAvatarIndex(map);
        if (avatarIndex == -1)
        {
            return nextMap; // no effect
        }
        int nextAvatarIndex = GetNextAvatarIndex(action, avatarIndex);
        //System.out.println(avatarIndex+" "+nextAvatarIndex);
        if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length)
        {
            if (nextMap[nextAvatarIndex] != '#')
            {
                // change the map
                nextMap[avatarIndex] = ' ';
                nextMap[nextAvatarIndex] = '@';
            }
        }
        return nextMap;
    }

    public char[] GetNextGhostState(int action)
    {
        char[] nextMap = (char[])map.Clone();
        // get location of '#'
        int ghostIndex = GetGhostIndex(map);
        if (ghostIndex == -1)
        {
            return nextMap; // no effect
        }
        int nextGhostIndex = GetNextAvatarIndex(action, ghostIndex);
        //System.out.println(avatarIndex+" "+nextAvatarIndex);
        if (nextGhostIndex >= 0 && nextGhostIndex < map.Length)
        {
            if (nextMap[nextGhostIndex] != 'G' && nextMap[nextGhostIndex] != '@')
            {
                // change the map
                nextMap[ghostIndex] = ' ';
                nextMap[nextGhostIndex] = '#';
            }
        }
        return nextMap;
    }

    public char[] GetNextGhostState(int action, char[] map)
    {
        char[] nextMap = (char[])map.Clone();
        // get location of '#'
        int ghostIndex = GetGhostIndex(map);
        if (ghostIndex == -1)
        {
            return nextMap; // no effect
        }
        int nextGhostIndex = GetNextAvatarIndex(action, ghostIndex);
        //System.out.println(avatarIndex+" "+nextAvatarIndex);
        if (nextGhostIndex >= 0 && nextGhostIndex < map.Length)
        {
            if (nextMap[nextGhostIndex] != 'G' && nextMap[nextGhostIndex] != '@')
            {
                // change the map
                nextMap[ghostIndex] = ' ';
                nextMap[nextGhostIndex] = '#';
            }
        }
        return nextMap;
    }

    public void GoToNextState(int action)
    {
        map = GetNextState(action);
    }

    public void GoToNextGhostState(int action)
    {
        map = GetNextGhostState(action);
    }

    public bool IsValidMove(int action)
    {
        int avatarIndex = GetAvatarIndex(map);
        if (avatarIndex == -1)
        {
            return false; // no effect
        }
        int nextAvatarIndex = GetNextAvatarIndex(action, avatarIndex);
        if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length && avatarIndex != nextAvatarIndex)
        {
            if (map[nextAvatarIndex] != '#')
            {
                return true;
            }
        }
        return false;
    }

    public bool IsValidMove(int action, char[] map)
    {
        int avatarIndex = GetAvatarIndex(map);
        if (avatarIndex == -1)
        {
            return false; // no effect
        }
        int nextAvatarIndex = GetNextAvatarIndex(action, avatarIndex);
        if (nextAvatarIndex >= 0 && nextAvatarIndex < map.Length && avatarIndex != nextAvatarIndex)
        {
            if (map[nextAvatarIndex] != '#')
            {
                return true;
            }
        }
        return false;
    }

    public bool IsValidGhostMove(int action, char[] map)
    {
        int ghostIndex = GetGhostIndex(map);
        if (ghostIndex == -1)
        {
            return false; // no effect
        }
        int nextGhostIndex = GetNextAvatarIndex(action, ghostIndex);
        if (nextGhostIndex >= 0 && nextGhostIndex < map.Length
                && ghostIndex != nextGhostIndex && nextGhostIndex != GetGoalIndex(map))
        {
            if (map[nextGhostIndex] != '@')
            {
                return true;
            }
        }
        return false;
    }

    public bool IsValidGhostMove(int action)
    {
        int ghostIndex = GetGhostIndex(map);
        if (ghostIndex == -1)
        {
            return false; // no effect
        }
        int nextGhostIndex = GetNextAvatarIndex(action, ghostIndex);
        if (nextGhostIndex >= 0 && nextGhostIndex < map.Length
                && ghostIndex != nextGhostIndex && nextGhostIndex != GetGoalIndex(map))
        {
            if (map[nextGhostIndex] != '@')
            {
                return true;
            }
        }
        return false;
    }

    public int GetAvatarIndex()
    {
        int avatarIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '@') { avatarIndex = i; }
        }
        return avatarIndex;
    }

    public int GetAvatarIndex(char[] map)
    {
        int avatarIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '@') { avatarIndex = i; }
        }
        return avatarIndex;
    }

    public int GetGhostIndex(char[] map)
    {
        int ghostIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '#') { ghostIndex = i; }
        }
        return ghostIndex;
    }

    public int GetGhostIndex()
    {
        int ghostIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '#') { ghostIndex = i; }
        }
        return ghostIndex;
    }

    public int GetGoalIndex(char[] map)
    {
        int goalIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == '#') { goalIndex = i; }
        }
        return goalIndex;
    }

    public bool IsGoalReached()
    {
        int goalIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 'G') { goalIndex = i; }
        }
        return (goalIndex == -1);
    }

    public bool IsAvatarDead(char[] map)
    {

        int currentAvatarIndex = GetAvatarIndex(map);
        int currentGhostIndex = GetGhostIndex(map);

        int avatarx = currentAvatarIndex % 8;
        int avatary = currentAvatarIndex / 8;

        int ghostx = currentGhostIndex % 8;
        int ghosty = currentGhostIndex / 8;

        if (Mathf.Abs(avatarx - ghostx) < 2 && Mathf.Abs(avatary - ghosty) < 2)
        {
            return true;
        }

        return false;

    }

    public bool IsGoalReached(char[] map)
    {
        int goalIndex = -1;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 'G') { goalIndex = i; }
        }
        return (goalIndex == -1);
    }

    public int GetNextAvatarIndex(int action, int currentAvatarIndex)
    {
        int x = currentAvatarIndex % 8;
        int y = currentAvatarIndex / 8;
        if (action == UP)
        {
            y--;
        }
        if (action == RIGHT)
        {
            x++;
        }
        else if (action == DOWN)
        {
            y++;
        }
        else if (action == LEFT)
        {
            x--;
        }
        if (x < 0 || y < 0 || x >= 8 || y >= 8)
        {
            return currentAvatarIndex; // no move
        }
        return x + 8 * y;
    }

    public void PrintMap()
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (i % 8 == 0)
            {
                Console.WriteLine("+-+-+-+-+-+-+-+-+");
            }
            Console.Write("|" + map[i]);
            if (i % 8 == 7)
            {
                Console.Write("|");
            }
        }
        Console.WriteLine("+-+-+-+-+-+-+-+-+");
    }

    public void PrintMap(char[] map)
    {
        for (int i = 0; i < map.Length; i++)
        {
            if (i % 8 == 0)
            {
                Console.WriteLine("+-+-+-+-+-+-+-+-+");
            }
            Console.Write("|" + map[i]);
            if (i % 8 == 7)
            {
                Console.Write("|");
            }
        }
        Console.WriteLine("+-+-+-+-+-+-+-+-+");
    }

    public string GetMoveName(int action)
    {
        string result = "ERROR";
        if (action == UP)
        {
            result = "UP";
        }
        else if (action == RIGHT)
        {
            result = "RIGHT";
        }
        else if (action == DOWN)
        {
            result = "DOWN";
        }
        else if (action == LEFT)
        {
            result = "LEFT";
        }
        return result;
    }

    public int GetGoodGhostAction(char[] map)
    {
        int currentAvatarIndex = GetAvatarIndex(map);
        int currentGhostIndex = GetGhostIndex(map);

        int avatarx = currentAvatarIndex % 8;
        int avatary = currentAvatarIndex / 8;

        int ghostx = currentGhostIndex % 8;
        int ghosty = currentGhostIndex / 8;

        int manhatanDistance = Mathf.Abs(ghosty - avatary) + Mathf.Abs(ghostx - avatarx);

        int goodAction = 0;
        for (int i = 0; i < 4; i++)
        {
            int possition = GetNextAvatarIndex(i, currentGhostIndex);
            ghostx = possition % 8;
            ghosty = possition / 8;
            int newmanhatan = Mathf.Abs(ghosty - avatary) + Mathf.Abs(ghostx - avatarx);
            if (newmanhatan < manhatanDistance)
            {
                goodAction = i;
                manhatanDistance = newmanhatan;
            }
        }
        return goodAction;
    }
}
