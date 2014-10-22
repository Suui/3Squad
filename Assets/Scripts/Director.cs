using UnityEngine;
using System.Collections;

namespace Medusa
{

    #region Event Handler Declaration

    public delegate void BoardOnNew(Board board);

    #endregion

    public class Director : MonoBehaviour
    {

        #region Inspector

        public int boardRows;
        public int boardColumns;

        public GameObject terrainPrefab;

        #endregion

        #region Event

        public event BoardOnNew OnNewBoard;

        #endregion

        #region Singleton

        private static Director instance;

        public static Director Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<Director>();
                return instance;
            }
        }

        #endregion

        #region Public Board

        public Board CurrentBoard
        {
            get;
            private set;
        }

        #endregion

        #region Start / Update

        void Start()
        {
            RecreateBoard();
        }

        void Update()
        {
            if (boardRows != CurrentBoard.Rows || boardColumns != CurrentBoard.Columns)
                RecreateBoard();
        }

        #endregion

        #region Board Creation

        private void RecreateBoard()
        {

            #region Destroy Old

            if (CurrentBoard != null)
            {
                GameObject.Destroy(CurrentBoard.SceneNode);
            }

            #endregion

            #region Create Scene Node

            CurrentBoard = new Board(boardRows, boardColumns, "Terrain", "Tokens", "Effects", "Overlay");

            #endregion
        
            #region Put Terrain

            foreach (Position pos in Position.Range(CurrentBoard))
            {
                GameObject go = (GameObject)Instantiate(terrainPrefab);
                go.name = "Cell @ " + pos;
                CurrentBoard ["Terrain"] [pos] = go;
                go.transform.position = (Vector3)pos;
            }

            #endregion

            #region Call Event

            if (OnNewBoard != null)
            {
                OnNewBoard(CurrentBoard);
            }

            #endregion

        }

        #endregion
	
    }
}