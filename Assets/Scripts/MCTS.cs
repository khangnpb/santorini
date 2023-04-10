//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MCTS
//{
   
//    int numSimulations = 3;
//    int maxDepth = 3;

//    public class Node
//    {
//        public Player player;
//        public int score;
//        public Dictionary<Player,State,Board,Node>[] ChildNodes;

//        public Node(int num)
//        {
//            for (int i = 0; i < num; i++)
//            {
//                ChildNodes[i] = new Dictionary<State, Node>();
//            }
//        }
//    }

//    Board board;
//    Player player;
//    Dictionary<State, Node> tree;


//    public MCTS(int numSimulations, int maxDepth, Board board, Player player , State statenow)
//    {
//        this.numSimulations = numSimulations;
//        this.maxDepth = maxDepth;
//        this.player = player;
//        this.tree = new Dictionary<State, Node> { { statenow, new Node(numSimulations) } };

//    }


//}