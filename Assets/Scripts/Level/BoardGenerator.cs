using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;


namespace Medusa
{

    class BoardGenerator
    {

        private Board board;

        private readonly GameObject boardCellPrefab;
        private readonly int boardRows, boardColumns;
        private readonly List<Position> possiblePositions;


        public BoardGenerator(GameObject boardCellPrefab, int boardRows, int boardColumns)
        {
            this.boardCellPrefab = boardCellPrefab;
            this.boardRows = boardRows;
            this.boardColumns = boardColumns;
            possiblePositions = new List<Position>(boardRows * boardColumns / 2);
        }


        public void CreateEmptyBoard(float boardYSize)
        {
            board = new Board(boardRows, boardColumns, "terrain", "tokens", "effects", "overlays");

            foreach (Position pos in board.Positions)
            {
                GameObject go = Object.Instantiate(boardCellPrefab) as GameObject;
                go.name = "cell " + pos;
                go.transform.position = new Vector3(pos.Row, -boardYSize, pos.Column);

                board["terrain"][pos] = go;
            }
        }


        // TODO: Make a list so that randoms never collide. Set all the obstacle types with a single seed.
        public void SpawnObstacles(GameObject[] obstacles, int limit, int seed)
        {
            FillPossiblePositions();

            int obstacleIndex;
            System.Random randPos = new System.Random(seed);
            System.Random randObs = new System.Random(seed);

            while (limit > 0)
            {
                obstacleIndex = randObs.Next(1000) % 2;

                Position pos = GetUniquePosition(randPos.Next(0, possiblePositions.Count));
                if (pos == null)
                    break;

                if (board["tokens"][pos] == null)
                {
                    // Initial instantiation
                    GameObject go = Object.Instantiate(obstacles[obstacleIndex]) as GameObject;
                    go.name = "token " + pos;
                    go.transform.position = pos;
                    board["tokens"][pos] = go;

                    // Symetric Instantiation
                    Position symetricPos = new Position(board.rows - 1 - pos.Row , board.columns - 1 - pos.Column);

                    GameObject symetricGO = Object.Instantiate(obstacles[obstacleIndex]) as GameObject;
                    symetricGO.name = "token " + symetricPos;
                    symetricGO.transform.position = symetricPos;
                    board["tokens"][symetricPos] = symetricGO;

                    limit--;
                }
            }
        }


        private void FillPossiblePositions()
        {
            for (int x = 0; x < boardRows; x++)
            {
                for (int z = 2; z < boardColumns / 2; z++)
                    possiblePositions.Add(new Position(x, z));
            }
        }


        private Position GetUniquePosition(int index)
        {
            if (possiblePositions.Count == 0)
            {
                Debug.LogWarning("There will only be placed " + (boardRows*boardColumns/2 - 2*boardRows) +
                                 " obstacles as limit");
                return null;
            }

            Position pos = possiblePositions[index];
            possiblePositions.RemoveAt(index);

            return pos;
        }


        public Board Board
        {
            get { return board; }
        }
    } 
}