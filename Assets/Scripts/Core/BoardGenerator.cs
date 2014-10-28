using UnityEngine;


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


        public void CreateBoard()
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


        public Board Board
        {
            get { return board; }
        }
    } 
}