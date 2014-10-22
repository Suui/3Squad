using UnityEngine;
using System;
using System.Collections.Generic;

namespace Medusa
{
    public class Board : Dimension
    {
        #region Basic Properties

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

        public GameObject SceneNode
        {
            get;
            private set;
        }

        #endregion

        #region Private Stuff

        private Dictionary<string,Layer> layers;

        #endregion

        #region Constructor

        public Board(int rows, int columns, params string[] names)
        {
            Rows = rows;
            Columns = columns;
            SceneNode = new GameObject("BoardNode");
            layers = new Dictionary<string,Layer >();
            foreach (string name in names)
            {
                AddLayer(name);
            }
        }

        #endregion

        #region Iterator Layers

        public IEnumerable<Layer> Layers
        {
            get
            {
                return layers.Values;
            }
        }

        #endregion

        #region Index

        public Layer this [string name]
        {
            get
            {
                if (!layers.ContainsKey(name))
                    throw new ArgumentException("Board doesn't contain any layer '" + name + "'");
                return layers [name];
            }
        }

        #endregion

        #region Layer Interaction

        public Layer AddLayer(string name)
        {
            Layer lay = new Layer(Rows, Columns, name);
            lay.SceneNode.transform.parent = SceneNode.transform;
            layers [name] = lay;
            return lay;
        }

        public Layer FindLayer(GameObject go)
        {
            return this [go.transform.parent.name];
        }

        #endregion
    }
}