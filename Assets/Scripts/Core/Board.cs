using UnityEngine;
using System;
using System.Collections.Generic;

namespace Medusa
{


    public delegate void BoardOnChange(Board caller,Layer layer,Position pos,GameObject oldGO,GameObject newGO);


    public class Board
    {

        public event BoardOnChange OnChange;
        public int rows, columns;

        private Dictionary<string,Layer> layers;


        public Board(int rows, int columns, params string[] names)
        {
            this.rows = rows;
            this.columns = columns;

            SceneNode = new GameObject("BoardNode");                    // Verify

            layers = new Dictionary<string,Layer >();
            foreach (string name in names)
            {
                AddLayer(name);
            }
        }


        public void AddLayer(string name)
        {
            Layer layer = new Layer(this, rows, columns, name);

            layer.SceneNode.transform.parent = SceneNode.transform;     // Verify
            layers[name] = layer;                                       // Verify

            layer.OnChange += LayerChangeEventHandler;                  // Event
        }


        public Layer this[string name]
        {
            get
            {
                if (layers.ContainsKey(name) == false)
                    throw new ArgumentException("The board does not contain a layer named '" + name + "'");

                return layers[name];
            }
        }


        public Layer GetLayerOf(GameObject gameObject)
        {
            return this[gameObject.transform.parent.name];          // This of a way of implementing this w/o depending on the GO structure
        }


        private void LayerChangeEventHandler(Layer layer, Position position, GameObject oldGameObject, GameObject newGameObject)
        {
            if (OnChange != null)
                OnChange(this, layer, position, oldGameObject, newGameObject);
        }


        // What does this?
        public void ValidatePosition(GameObject gameObject)
        {
            Position pos = GetLayerOf(gameObject).Find(gameObject);

            if (pos != null)
                gameObject.transform.position = pos;        // Redundant cast to Vector3
        }


        public bool CheckIndex(Position position)
        {
            return position.x >= 0
                   && position.z >= 0
                   && position.x < rows
                   && position.z < columns;
        }


        #region Getters and Setters

        public int Rows
        {
            get;
            private set;
        }


        public int Columns
        {
            get;
            private set;
        }


        public IEnumerable<Layer> Layers
        {
            get { return layers.Values; }
        }


        public GameObject SceneNode
        {
            get;
            private set;
        }

        #endregion


    }
}