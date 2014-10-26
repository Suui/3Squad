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


        public Layer(int rows, int columns, string name)
        {
            gameObjects = new GameObject[rows, columns];
            Name = name;

            SceneNode = new GameObject(name);
        }


        public GameObject this [Position position]
        {
            get
            { 
                if (position == null)
                    throw new ArgumentOutOfRangeException("Trying to access null");
                if (position.Outside(this))
                    throw new ArgumentOutOfRangeException(position + " not in layer");

                return gameObjects [position.Row, position.Column]; 
            }

            set
            {
                // Argument Validation

                if (position == null)
                    throw new ArgumentOutOfRangeException("Trying to access null");
                if (position.Outside(this))
                    throw new ArgumentOutOfRangeException(position + " not in layer");


                // Handle Old & Transform

                GameObject old = this [position];

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


        private bool CheckIndex(Position position)
        {
            return position.
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