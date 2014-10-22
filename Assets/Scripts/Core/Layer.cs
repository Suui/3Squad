using UnityEngine;
using System;
using System.Collections.Generic;

namespace Medusa
{

    public class Layer : Dimension
    {
   
        #region Basic Properties

        public int Rows
        {
            get { return container.GetLength(0); }
        }

        public int Columns
        {
            get { return container.GetLength(1); }
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

        #endregion

        #region Private Stuff

        private GameObject[,] container;

        #endregion

        #region Iterator Objects
        
        public IEnumerable<GameObject> Objects
        {
            get
            {
                foreach (GameObject go in container)
                {
                    if (go != null)
                    {
                        yield return go;
                    }
                }
            }
        }

        #endregion

        #region Contructor

        public Layer(int rows, int columns, string name)
        {
            container = new GameObject[rows, columns];
            Name = name;
            SceneNode = new GameObject(name);
        }

        #endregion

        #region Index
    
        public GameObject this [Position pos]
        {
            get
            { 
                if (pos == null)
                    throw new ArgumentOutOfRangeException("Trying to access null");
                if (pos.Outside(this))
                    throw new ArgumentOutOfRangeException(pos + " not in layer");
                return container [pos.Row, pos.Column]; 
            }
            set
            {
                if (pos == null)
                    throw new ArgumentOutOfRangeException("Trying to access null");
                if (pos.Outside(this))
                    throw new ArgumentOutOfRangeException(pos + " not in layer");
                GameObject old = this [pos];
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
                container [pos.Row, pos.Column] = value;
            }
        }
    
        #endregion

        #region Basic Methods

        public bool Empty(Position pos)
        {
            return this [pos] == null;
        }

        public void Clear()
        {
            foreach (GameObject go in Objects)
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

        #endregion

        #region Component Interaction

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

        public T[] All<T>(Position pos) where T :Component
        {
            GameObject go = this [pos];
            if (go == null)
                return null;
            T[] items = go.GetComponents<T>();
            return items;
        }
        
        
        #endregion       
       
        #region Utilities

        public IEnumerable<GameObject> Where(Func<GameObject,bool> test)
        {
            foreach (GameObject go in Objects)
            {
                if (test(go))
                {
                    yield return go;
                }
            }
        }

        #endregion

    }
}