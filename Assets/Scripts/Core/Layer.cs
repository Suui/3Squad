using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;


namespace Medusa
{

    public class Layer
    {

        private readonly Dictionary<Position, GameObject> gameObjects;
        private readonly Board board;
        private readonly string name;


        public Layer(Board board, string name)
        {
            this.board = board;
            this.name = name;

            gameObjects = new Dictionary<Position, GameObject>();
            
            InitializeDictionary();
            SceneNode = new GameObject(name);
        }


        private void InitializeDictionary()
        {
            // Master's Positions
            gameObjects.Add(new Position(board.Rows / 2, -2), null);
            gameObjects.Add(new Position(board.Rows / 2, board.Columns + 1), null);

            for (int x = 0; x < board.Rows; x++)
            {
                for (int z = 0; z < board.Columns; z++)
                    gameObjects.Add(new Position(x, z), null);
            }
        }


        public GameObject this[Position position]
        {
            get
            {
                if (position == null)
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                return gameObjects[position]; 
            }


            set
            {
                if (position == null)
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                gameObjects[position] = value;

                if (value != null)
                    value.transform.parent = SceneNode.transform;
            }
        }


        // TODO: Does this set the position as null? It should.
        public void RemoveGameObjectAt(Position position)
        {
            if (IsEmpty(position) == false)
            {
                Object.Destroy(this[position]);
                this[position] = null;
            }
        }


        public void RemoveGameObject(GameObject gameObject)
        {
            if (Contains(gameObject))
                RemoveGameObjectAt(GetPositionOf(gameObject));
            else
                throw new ArgumentException("The GameObject " + gameObject.name + " is not in the Layer");
        }


        public void AddGameObjectAt(GameObject gameObject, Position position)
        {
            this[position] = gameObject;
			gameObject.transform.position = position;
        }


        public void MoveGameObject(GameObject gameObject, Position to)
        {
            this[to] = gameObject;
            this[(Position) gameObject.transform] = null;

            gameObject.transform.position = to;
        }

		public void SwitchGameObjects(GameObject first, GameObject second)
		{
			GameObject one = first;
			Position secondPos = (Position) second.transform;
			
			this[(Position) one.transform] = null;
			
			MoveGameObject(second, (Position) first.transform);
			
			this[secondPos] = first;
			first.transform.position = secondPos;
		}

        public void SwitchGameObjects(GameObject first, GameObject second)
        {
            GameObject one = first;
            Position secondPos = (Position) second.transform;

            this[(Position) one.transform] = null;

            MoveGameObject(second, (Position) first.transform);

            this[secondPos] = first;
            first.transform.position = secondPos;
        }


        public bool IsEmpty(Position position)
        {
            return this [position] == null;
        }


        public void ClearLayer()
        {
            foreach (GameObject go in GameObjects)
            {
                Object.Destroy(go);
            }
        }


        public bool Contains(GameObject gameObject)
        {
            return gameObject.transform.parent == SceneNode.transform;
        }


        public Position GetPositionOf(GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            Position pos;
            for (int x = 0, rows = board.Rows; x < rows; x++)
            {
                for (int z = 0, columns = board.Columns; z < columns; z++)
                {
                    pos = new Position(x, z);

                    if (gameObjects[pos] == gameObject)
                        return pos;
                }
            }

            return null;
        }


        public bool HasComponent<T>(Position pos) where T : Component
        {
            GameObject go = this[pos];

            if (go == null)
                return false;

            T item = go.GetComponent<T>();
            return (item != null);
        }


        public T GetComponent<T>(Position pos) where T : Component
        {
            GameObject go = this[pos];

            if (go == null)
                return null;

            T item = go.GetComponent<T>();
            return item;
        }


        public T[] GetAllComponents<T>(Position pos) where T : Component
        {
            GameObject go = this[pos];

            if (go == null)
                return null;

            T[] items = go.GetComponents<T>();
            return items;
        }


        // Lambda
        public IEnumerable<GameObject> Where(Func<GameObject, bool> test)
        {
            return GameObjects.Where(test);
        }


        #region Getters and Setters

        public GameObject SceneNode
        {
            get;
            private set;
        }


        public string Name
        {
            get { return name; }
        }


        public IEnumerable<GameObject> GameObjects
        {
            get {
                return gameObjects.Values.Where(go => go != null);
            }
        }


        public IEnumerable<Position> Positions
        {
            get
            {
                var buffer = new List<Position>(gameObjects.Keys);
                return buffer;
            }
        }

        #endregion


        public bool Outside (Position position)
        {
            return position.Row < 0
                || position.Row >= board.rows
                || position.Column < 0
                || position.Column >= board.columns;
        }

    }
}