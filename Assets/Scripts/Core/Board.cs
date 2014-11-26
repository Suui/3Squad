using UnityEngine;
using System;
using System.Collections.Generic;


namespace Medusa
{

    public class Board
    {

        public readonly int rows, columns;

        private readonly Dictionary<string,Layer> layers;
        private readonly List<Position> positions;


        public Board(int rows, int columns, params string[] names)
        {
            this.rows = rows;
            this.columns = columns;
            positions = new List<Position>(rows * columns);

            SceneNode = new GameObject("BoardNode")
			{ tag = "BoardNode" };

            layers = new Dictionary<string, Layer>();
            foreach (string name in names)
            {
                AddLayer(name);
            }
        }


        public void AddLayer(string name)
        {
            Layer layer = new Layer(this, name);

            layer.SceneNode.transform.parent = SceneNode.transform;
            layers[name] = layer;
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
                if (positions.Count == 0)
                    FillPositions();

                return positions;
            }
        }


        private void FillPositions()
        {
            for (int x = 0; x < rows; x++)
            {
                for (int z = 0; z < columns; z++)
                    positions.Add(new Position(x, z));
            }
        }

        #endregion

    }
}