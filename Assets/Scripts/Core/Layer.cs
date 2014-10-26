using UnityEngine;
using System;
using System.Collections.Generic;


namespace Medusa
{

    public delegate void LayerOnChange(Layer caller,Position pos,GameObject oldGO,GameObject newGO);


    public class Layer
    {

        public event LayerOnChange OnChange;

        private GameObject[,] gameObjects;
        private Board board;
        private string name;


        public Layer(Board board, int rows, int columns, string name)
        {
            this.board = board;
            this.name = name;

            gameObjects = new GameObject[rows, columns];
            SceneNode = new GameObject(name);
        }


        public GameObject this[Position position]
        {
            get
            { 
                if (position == null)                           // Return null instead of exception?
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                if (board.CheckIndex(position) == false)        // Return null instead of exception?
                    throw new ArgumentOutOfRangeException("The position " + position + " is out of range");

                return gameObjects[position.x, position.z]; 
            }

            // TODO: Functionality of set: Are we able to replace an object? Would be better to have a function to do that
            // Only able to set if the position you are trying to establish is empty
            set
            {
                if (position == null)
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                if (board.CheckIndex(position) == false)
                    throw new ArgumentOutOfRangeException("The position " + position + " is out of range");

                if (this[position] == null)
                {
                    gameObjects[position.x, position.z] = value;
                    value.transform.parent = SceneNode.transform;

                    if (OnChange != null)
                        OnChange(this, position, null, value);
                }
                else
                    throw new ArgumentException("Cannot set " + position + " as it is already occupied by " + this[position].name);
            }
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

        // TODO: == or = ?
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
                {
                    yield return go;
                }
            }
        }


        #region Getters and Setters

        // Do we need this?
        public int Rows
        {
            get { return gameObjects.GetLength(0); }
        }


        // Do we need this?
        public int Columns
        {
            get { return gameObjects.GetLength(1); }
        }


        public GameObject SceneNode
        {
            get;
            private set;
        }


        public string Name
        {
            get;
            private set;
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