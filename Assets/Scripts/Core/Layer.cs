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

            set
            {
                if (position == null)
                    throw new ArgumentOutOfRangeException("The parameter position is null");

                if (board.CheckIndex(position) == false)
                    throw new ArgumentOutOfRangeException("The position " + position + " is out of range");

                // Handle Old & Transform

                GameObject old = this[position];

                if (value == null)
                {
                    if (old == null)
                        throw new ArgumentException("Removing a Nothing");

                } else
                {
                    if (old != null)
                    {
                        old.transform.parent = null;
                    }
                    value.transform.parent = SceneNode.transform;
                }


                // Set New Value & Call Event

                gameObjects [position.Row, position.Column] = value;
                if (OnChange != null)
                    OnChange(this, position, old, value);

            }
        }


        public bool Empty(Position pos)
        {
            return this [pos] == null;
        }


        public void Clear()
        {
            foreach (GameObject go in GameObjects)
            {
                GameObject.Destroy(go);
            }
        }


        public bool Contains(GameObject go)
        {
            return go.transform.parent = SceneNode.transform;
        }


        public Position Find(GameObject go)
        {
            if (go == null)
                return null;
            foreach (Position pos in Position.Range(this))
            {
                if (this [pos] == go)
                    return pos; 
            }
            return null;
        }


        public bool Has<T>(Position pos) where T : Component
        {
            GameObject go = this [pos];
            if (go == null)
                return false;
            T item = go.GetComponent<T>();
            return (item != null);
        }


        public T Get<T>(Position pos) where T : Component
        {
            GameObject go = this [pos];
            if (go == null)
                return null;
            T item = go.GetComponent<T>();
            return item;
        }


        public T[] All<T>(Position pos) where T : Component
        {
            GameObject go = this [pos];
            if (go == null)
                return null;
            T[] items = go.GetComponents<T>();
            return items;
        }


        // Lambda
        public IEnumerable<GameObject> Where(Func<GameObject,bool> test)
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