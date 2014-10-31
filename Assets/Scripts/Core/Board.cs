using UnityEngine;
using System;
using System.Collections.Generic;


namespace Medusa
{

    public delegate void BoardOnChange(Board caller,Layer layer,Position pos,GameObject oldGO,GameObject newGO);


    public class Board
    {

        public event BoardOnChange OnChange;
        public readonly int rows, columns;

        private readonly Dictionary<string,Layer> layers;


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


        // TODO: Verify SceneNode and Event
        public void AddLayer(string name)
        {
            Layer layer = new Layer(this, name);

            layer.SceneNode.transform.parent = SceneNode.transform;
            layers[name] = layer;

            layer.OnChange += LayerChangeEventHandler;
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


        // TODO: Search a way to implement this w/o depending on the GO structures -- Not needed right now
        public Layer GetLayerOf(GameObject gameObject)
        {
            return this[gameObject.transform.parent.name];
        }


        public void RefreshPositionOf(GameObject gameObject)
        {
            Position pos = GetLayerOf(gameObject).GetPositionOf(gameObject);

            if (pos != null)
                gameObject.transform.position = pos;
        }


        public IEnumerable<Position> Way(Position originPosition, Direction direction, int range = Int32.MaxValue)
        {
            Position pos = originPosition;

            while (IsInside(pos += direction) && range-- > 0)
                yield return pos;
        }


        public bool IsInside(Position position)
        {
            return position.Row >= 0
                   && position.Column >= 0
                   && position.Row < rows
                   && position.Column < columns;
        }


        private void LayerChangeEventHandler(Layer layer, Position position, GameObject oldGameObject, GameObject newGameObject)
        {
            if (OnChange != null)
                OnChange(this, layer, position, oldGameObject, newGameObject);
        }


        #region Getters and Setters

        public int Rows
        {
            get { return rows; }
        }


        public int Columns
        {
            get { return columns; }
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


        public IEnumerable<Position> Positions
        {
            get
            {
                for (int x = 0; x < rows; x++)
                {
                    for (int z = 0; z < columns; z++)
                        yield return new Position(x, z);
                }
            }
        }

        #endregion

    }
}