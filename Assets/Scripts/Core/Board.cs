using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;


namespace Medusa
{

    public delegate void BoardOnChange(Board caller,Layer layer,Position pos,GameObject oldGO,GameObject newGO);


    public class Board
    {

        public event BoardOnChange OnChange;
        public readonly int rows, columns;

        private Dictionary<Position, GameObject> masters;
        private readonly Dictionary<string,Layer> layers;
        private readonly List<Position> positions;


        public Board(int rows, int columns, params string[] names)
        {
            this.rows = rows;
            this.columns = columns;
            positions = new List<Position>(rows * columns);
            masters = new Dictionary<Position, GameObject>(2);

            SceneNode = new GameObject("BoardNode");
            MastersNode = new GameObject("MastersNode");

            layers = new Dictionary<string, Layer>();
            foreach (string name in names)
            {
                AddLayer(name);
            }
        }


        public void PlaceMasters(GameObject masterOne, GameObject masterTwo, Position masterOnePos, Position masterTwoPos)
        {
            GameObject node = new GameObject("Masters");
            node.transform.parent = MastersNode.transform;

            GameObject master1 = Object.Instantiate(masterOne) as GameObject;
            master1.name = "Mater 01";
            master1.transform.position = masterOnePos;
            master1.transform.parent = node.transform;

            masters.Add(masterOnePos, masterOne);

            GameObject master2 = Object.Instantiate(masterTwo) as GameObject;
            master2.name = "Mater 02";
            master2.transform.position = masterTwoPos;
            master2.transform.parent = node.transform;

            masters.Add(masterTwoPos, masterTwo);

        }


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


        public Dictionary<Position, GameObject> Masters
        {
            get { return masters; }
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


        public GameObject MastersNode
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

                foreach (Position pos in positions)
                    yield return pos;
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