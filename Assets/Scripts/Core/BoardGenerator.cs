﻿using UnityEngine;


namespace Medusa
{

    class BoardGenerator
    {

        private Board board;

        private GameObject boardCellPrefab;
        private int boardRows, boardColumns;


        public BoardGenerator(GameObject boardCellPrefab, int boardRows, int boardColumns)
        {
            this.boardCellPrefab = boardCellPrefab;
            this.boardRows = boardRows;
            this.boardColumns = boardColumns;
        }


        public void CreateEmptyBoard()
        {
            board = new Board(boardRows, boardColumns, "terrain", "tokens", "effects", "overlays");

            foreach (Position pos in board.Positions())
            {
                GameObject go = Object.Instantiate(boardCellPrefab) as GameObject;
                go.name = "cell " + pos;
                go.transform.position = pos;

                board["terrain"][pos] = go;
            }
        }


        // TODO: Make a list so that randoms never collide. Set all the obstacle types with a single seed.
        public void SpawnObstacles(GameObject[] obstacles, int limit, int seed)
        {
            int x, z, obstacleIndex;
            System.Random randPos = new System.Random(seed);   // seed
            System.Random randObs = new System.Random(seed);   // seed

            while (limit > 0)
            {
                x = randPos.Next(0, board.Rows);
                z = randPos.Next(2, board.Columns / 2 - 1);
                obstacleIndex = randObs.Next(0, obstacles.Length);

                Position pos = new Position(x, z);

                if (board["tokens"][pos] == null)
                {
                    // Initial instantiation
                    GameObject go = Object.Instantiate(obstacles[obstacleIndex]) as GameObject;
                    go.name = "token " + pos;
                    go.transform.position = new Vector3(pos.X, 1.0f, pos.Z);
                    board["tokens"][pos] = go;

                    // Symetric Instantiation
                    Position symetricPos = new Position(board.rows - 1 - pos.X , board.columns - 1 - pos.Z);

                    GameObject symetricGO = Object.Instantiate(obstacles[obstacleIndex]) as GameObject;
                    symetricGO.name = "token " + symetricPos;
                    symetricGO.transform.position = new Vector3(symetricPos.X, 1.0f, symetricPos.Z);
                    board["tokens"][symetricPos] = symetricGO;

                    limit--;
                }
            }

            SymetrizeObstacles();

        }


        private void SymetrizeObstacles()
        {
            
        }


        public Board Board
        {
            get { return board; }
        }
    } 
}