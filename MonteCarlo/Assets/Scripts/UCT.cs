using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCT : MonoBehaviour
{
    // Singleton hack for Main coroutine, something weird is happening
    static bool running = false;

    /*
	 * Maze used to control the game
	 */
    public Map maze;

    /*
	 * rootNode is the starting point of the present state
	 */
    Node rootNode;

    /*
	 * currentNode refers to the node we work at every step
	 */
    Node currentNode;

    /*
	 * Exploration coefficient
	 */
    public float C = (float)(1.0 / Mathf.Sqrt(2));

    /*
	 * Computational limit
	 */
    public const int maxIterations = 100;

    public MapManager mapManager;

    /**
	 * Initialize the maze game
	 */
    void Awake()
    {
        maze = new Map();
        maze.ResetMaze();
    }

    /**
	 * run the UCT search and find the optimal action for the root node state
	 * @return
	 * @throws InterruptedException
	 */
    public int RunUCT()
    {

        /*
         * Create root node with the present state
         */
        rootNode = new Node((char[])maze.map.Clone());

        /*
         * Apply UCT search inside computational budget limit (default=100 iterations) 
         */
        int iterations = 0;
        while (!Terminate(iterations))
        {
            iterations++;

            //TODO Implement UCT algorithm here
            throw new UnityException("You didn't implement something!");
        }

        /*
         * Get the action that directs to the best node
         */
        currentNode = rootNode;
        //rootNode is the one we are working with 
        //and we apply the exploitation of it to find the child with the highest average reward
        int bestAction = 0;
        float bestReward = 0;
        // TODO calculate which is the best action
        throw new UnityException("You didn't implement something!");

        return bestAction;
    }

    /**
	 * Expand the nonterminal nodes with one available child. 
	 * Chose a node to expand with BestChild(C) method
	 */
    private void TreePolicy()
    {
        //TODO implement the tree policy
        throw new UnityException("You didn't implement something!");

    }

    /**
     * Check if the node is fully expanded
     * @param nt
     * @return
     */
    private bool FullyExpanded(Node nt)
    {
        // TODO check if the node nt is fully expanded or not
        throw new UnityException("You didn't implement something!");
        return false;
    }

    /**
     * Expand the current node by adding new child to the currentNode
     */
    private void Expand()
    {
        /*
         * Choose untried action
         */
        int action = UntriedAction(currentNode);

        /*
         * Create a child, set its fields and add it to currentNode.children
         */
        Node child = new Node(maze.GetNextGhostState(RandomGhostAction(maze.GetNextState(action, currentNode.state)),
                maze.GetNextState(action, currentNode.state)));

        // TODO: add child to the children of the current node, set parent and parentAction of child, change currentNode
        throw new UnityException("You didn't implement something!");

    }

    /**
     * Choose the best child according to the UCT value
     * Assign it as a currentNode
     * @param c Exploration coefficient
     */
    private void BestChild(float c)
    {
        Node nt = currentNode;
        Node bestChild = null;

        // TODO find best child
        throw new UnityException("You didn't z something!");

        currentNode = bestChild;
    }

    /**
     * Calculate UCT value for the best child choosing
     * @param n child node of currentNode
     * @param c Exploration coefficient
     * @return
     */
    private float UCTvalue(Node n, float c)
    {
        // TODO: calculate the UCT value for the node n ad return it
        throw new UnityException("You didn't implement something!");
        return 0;
    }

    /**
     * Simulation of the game. Choose random actions up until the game is over (goal reached or dead)
     * @return reward (1 for win, 0 for loss)
     */
    private float DefaultPolicy()
    {
        char[] st = (char[])currentNode.state.Clone();
        while (!TerminalState(st))
        {
            int action = RandomAction(st);
            st = maze.GetNextState(action, st);
            int ghostAction = RandomGhostAction(st);
            st = maze.GetNextGhostState(ghostAction, st);
        }
        return maze.GetReward(st);
    }

    /**
     * Assign the received reward to every parent of the parent up to the rootNode
     * Increase the visited count of every node included in backpropagation
     * @param reward
     */
    private void Backpropagate(float reward)
    {
        // TODO update the reward and timesvisited of the current node and all its parents until you reach the root of the tree
        throw new UnityException("You didn't implement something!");
    }

    /**
     * Check if the state is the end of the game
     * @param state
     * @return
     */
    private bool TerminalState(char[] state)
    {
        return maze.IsGoalReached(state) || maze.IsAvatarDead(state);
    }

    /**
     * Returns the first untried action of the node
     * @param n
     * @return
     */
    private int UntriedAction(Node n)
    {
        List<int> possibleActions = new List<int> { 0, 1, 2, 3 };
        for (int k = 0; k < n.children.Count; k++)
        {
            if (possibleActions.Contains(n.children[k].parentAction))
            {
                possibleActions.Remove(n.children[k].parentAction);
            }
        }
        if (possibleActions.Count == 0)
            return -1;
        else
        {
            int randomAction = possibleActions[Random.Range(0, possibleActions.Count)];
            return randomAction;
        }
    }

    /**
     * Check if the algorithm is to be terminated, e.g. reached number of iterations limit
     * @param i
     * @return
     */
    private bool Terminate(int i)
    {
        if (i > maxIterations) return true;
        return false;
    }

    /**
     * Used in game simulation to pick random action for the agent
     * @param state st
     * @return action
     */
    private int RandomAction(char[] st)
    {
        int action = Random.Range(0,4);
        while (!maze.IsValidMove(action, st))
        {
            action = Random.Range(0, 4);
        }
        return action;
    }

    /**
     * Used in game simulation to pick random action for the ghost
     * @param state st
     * @return action
     */
    private int RandomGhostAction(char[] st)
    {
        int action = Random.Range(0, 4);
        while (!maze.IsValidGhostMove(action, st))
        {
            action = Random.Range(0, 4);
        }
        return action;
    }

    /**
     * UCT maze solving test
     * @param args
     * @throws InterruptedException 
     */
    public IEnumerator Main() 
    {
        /* Something is weird here, the Main coroutine is called 2 times, but I can't figue out why!
         * I made a "singleton-ish hack" to be sure only one can run at a time, but it's not the best solution
         * If you can figure it out, let me know ;)
         */
        // START HACK
        Debug.Log("trying to start UCT");
        if (running)
            yield break;
        else running = true;
        // END HACK

        while (true){
            // PRINT MAP
			maze.PrintMap();
            mapManager.ArrangeMap(maze.map);
            // CHECK IF WON OR LOST, THEN RESET
            if(maze.IsGoalReached()){
                Debug.Log("GOAL REACHED");
                maze.ResetMaze();
                yield break;
            }
            
            if(maze.IsAvatarDead(maze.map)){
                Debug.Log("AVATAR DEAD");
                maze.ResetMaze();
                yield break;
            }
            
            //FIND THE OPTIMAL ACTION VIA UTC
            int bestAction = RunUCT();

            //ADVANCE THE GAME WITH MOVES OF AGENT AND GHOST
            maze.GoToNextState(bestAction);
            int bestGhostAction = Random.Range(0, 4);
            while (!maze.IsValidGhostMove(bestGhostAction)){
            	bestGhostAction = Random.Range(0, 4);
            }
            maze.GoToNextGhostState(bestGhostAction);

            //TRACK THE GAME VISUALY
            yield return new WaitForSeconds(1);
        }
		
	}

    void Start()
    {
        StartCoroutine("Main");
    }
}
