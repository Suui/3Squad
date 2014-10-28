using UnityEngine;


namespace Medusa
{

    public delegate void BoardOnNew(Board board);


    public class GameMaster : MonoBehaviour
    {

        public event BoardOnNew OnNewBoard;

        public GameObject boardCellPrefab;
        public int boardRows;
        public int boardColumns;

        private BoardGenerator boardGenerator;


        #region Singleton

        private static GameMaster instance;

        public static GameMaster Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameMaster>();
                return instance;
            }
        }

        #endregion


        void Start()
        {
            boardGenerator = new BoardGenerator(boardCellPrefab, boardRows, boardColumns);
            boardGenerator.CreateBoard();
        }


        void Update()
        {

        }


        public Board CurrentBoard
        {
            get { return boardGenerator.Board; }
        }

    }
}