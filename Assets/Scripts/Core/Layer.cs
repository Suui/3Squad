using UnityEngine;
using System;
using System.Collections.Generic;


namespace Medusa
{

    public delegate void LayerOnChange(Layer caller,Position pos,GameObject oldGO,GameObject newGO);


    public class Layer
    {

        public event LayerOnChange OnChange;

        private readonly GameObject[,] gameObjects;
        private readonly Board board;
        private readonly string name;


        public Layer(Board board, string name)
        {
            this.board = board;
            this.name = name;

            gameObjects = new GameObject[board.rows, board.columns];
            SceneNode = new GameObject(name);
        }


        public GameObject this[Position position]
        {
            get
            { 
                if (position == null)                           // Return null instead of exception?
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                if (board.IsInside(position) == false)        // Return null instead of exception?
                    throw new ArgumentOutOfRangeException("The position " + position + " is out of range");

                return gameObjects[position.X, position.Z]; 
            }

            // TODO: Functionality of set: Are we able to replace an object? Would be better to have a function to do that
            // Actually set to able set IF the position you are trying to establish is empty
            set
            {
                if (position == null)
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                if (board.IsInside(position) == false)
                    throw new ArgumentOutOfRangeException("The position " + position + " is out of range");

                if (this[position] == null)
                {
                    gameObjects[position.X, position.Z] = value;

                    if (value != null)
                        value.transform.parent = SceneNode.transform;

                    if (OnChange != null)
                        OnChange(this, position, null, value);
                }
                else
                    throw new ArgumentException("Cannot set " + position + " as it is already occupied by " + this[position].name);
            }
        }


        // TODO: Does this set the position as null? It should.
        public void RemoveGameObjectAt(Position position)
        {
            if (IsEmpty(position) == false)
                UnityEngine.Object.Destroy(this[position]);
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
        }


        public bool IsEmpty(Position position)
        {
            return this [position] == null;
        }


        public void ClearLayer()
        {
            foreach (GameObject go in GameObjects)
            {
                GameObject.Destroy(go);
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

            for (int x = 0, rows = board.Rows; x < rows; x++)
            {
                for (int z = 0, columns = board.Columns; z < columns; z++)
                {
                    if (gameObjects[x, z] == gameObject)
                        return new Position(x, z);
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
            foreach (GameObject go in GameObjects)
            {
                if (test(go))
                    yield return go;
            }
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
            get
            {
                foreach (GameObject go in gameObjects)
                {
                    if (go != null)
                    {
                        yield return go;
                    }
                }
            }
        }

        #endregion

    }
}