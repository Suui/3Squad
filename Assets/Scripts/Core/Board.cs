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


        // TODO: Verify SceneNode and Event
        public void AddLayer(string name)
        {
            Layer layer = new Layer(this, rows, columns, name);

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


        // TODO: Search a way to implement this w/o depending on the GO structures
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


        // TODO: Set new name.
        public IEnumerable<Position> GetPositions(Position originPosition, Direction direction, int range, params string[] affectedLayers)
        {
            Position pos = originPosition;

            while (CheckIndex(pos += direction) && range-- > 0)
                yield return pos;
        }


        public IEnumerable<Position> GetPositionsWithoutRange(Position originPosition, Direction direction, params string[] affectedLayers)
        {
            Position pos = originPosition;

            while (CheckIndex(pos += direction))
                yield return pos;
        }


        public bool CheckIndex(Position position)
        {
            return position.X >= 0
                   && position.Z >= 0
                   && position.X < rows
                   && position.Z < columns;
        }


        private void LayerChangeEventHandler(Layer layer, Position position, GameObject oldGameObject, GameObject newGameObject)
        {
            if (OnChange != null)
                OnChange(this, layer, position, oldGameObject, newGameObject);
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