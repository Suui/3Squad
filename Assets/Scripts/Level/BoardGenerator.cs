using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;


namespace Medusa
{

    class BoardGenerator
    {

        private Board board;

        private readonly GameObject boardCellPrefab;
        private readonly GameObject masterCellPrefab;
        private readonly int boardRows, boardColumns;
        private readonly List<Position> possiblePositions;


        public BoardGenerator(GameObject boardCellPrefab, GameObject masterCellPrefab, int boardRows, int boardColumns)
        {
            this.boardCellPrefab = boardCellPrefab;
            this.masterCellPrefab = masterCellPrefab;
            this.boardRows = boardRows;
            this.boardColumns = boardColumns;
            possiblePositions = new List<Position>(boardRows * boardColumns / 2);
        }


        public void CreateEmptyBoard(float boardYSize)
        {
            board = new Board(boardRows, boardColumns, "terrain", "tokens", "effects", "overlays", "TurnManagement");

            foreach (Position pos in board.Positions)
            {
                // Terrain
                GameObject cell = Object.Instantiate(boardCellPrefab) as GameObject;
                cell.name = "cell " + pos;
                cell.transform.position = new Vector3(pos.Row, -boardYSize, pos.Column);

                board["terrain"][pos] = cell;

                // Overlays
                GameObject overlay = Object.Instantiate(Resources.Load("Prefabs/Overlay_Prefab")) as GameObject;
                overlay.name = "overlay " + pos;
                overlay.transform.position = pos;

                board["overlays"][pos] = overlay;
            }

        }


        public void SetUpMasters(GameObject masterOne, GameObject masterTwo, float boardYSize)
        {
            GameObject Cells = new GameObject("Cells");
            Cells.transform.parent = board.MastersNode.transform;

            GameObject Overlays = new GameObject("Overlays");
            Overlays.transform.parent = board.MastersNode.transform;

            Position masterOnePos = new Position(boardRows / 2, -2);
            Position masterTwoPos = new Position(boardRows / 2, boardColumns + 1);


            GameObject masterCellOne = Object.Instantiate(masterCellPrefab) as GameObject;
            masterCellOne.name = "Master 01 Cell";
            masterCellOne.transform.position = new Vector3(masterOnePos.Row, -boardYSize, masterOnePos.Column);
            masterCellOne.transform.parent = Cells.transform;

            GameObject masterCellTwo = Object.Instantiate(masterCellPrefab) as GameObject;
            masterCellTwo.name = "Master 02 Cell";
            masterCellTwo.transform.position = new Vector3(masterTwoPos.Row, -boardYSize, masterTwoPos.Column);
            masterCellTwo.transform.parent = Cells.transform;


            GameObject overlayOne = Object.Instantiate(Resources.Load("Prefabs/Master_Overlay_Prefab")) as GameObject;
            overlayOne.name = "Master 01 Overlay";
            overlayOne.transform.position = masterOnePos;
            overlayOne.transform.parent = Overlays.transform;

            GameObject overlayTwo = Object.Instantiate(Resources.Load("Prefabs/Master_Overlay_Prefab")) as GameObject;
            overlayTwo.name = "Master 02 Overlay";
            overlayTwo.transform.position = masterTwoPos;
            overlayTwo.transform.parent = Overlays.transform;


            board.PlaceMasters(masterOne, masterTwo, masterOnePos, masterTwoPos);
        }


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