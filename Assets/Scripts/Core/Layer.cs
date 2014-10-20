using UnityEngine;
using System;
using System.Collections.Generic;

namespace Medusa
{

  public abstract class Layer
  {
    public abstract Type ItemType
    {
      get;
    }
  }

  public class Layer<T> : Layer where T : CellItem
  {

    private T[,] container;
   
    #region Iterators Items/Positions

    public IEnumerable<T> Items
    {
      get
      {
        foreach (T item in container)
        {
          if (item != null)
          {
            yield return item;
          }
        }
      }
    }

    public IEnumerable<Position> Positions
    {
      get
      {
        for (int x = 0, width = Width; x < width; x++)
        {
          for (int y = 0, height = Height; y < height; y++)
          {
            yield return new Position(x, y);
          }
        } 
      }
    }

    #endregion

    #region Basic Properties

    public int Width
    {
      get;
      private set;
    }

    public int Height
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

    public Layer(int width, int height)
    {
      Width = width;
      Height = height;
      container = new T[width, height];
      string[] itemType = ItemType.ToString().Split('.');
      SceneNode = new GameObject(itemType [itemType.Length - 1] + "Layer");
    }

    public override Type ItemType
    {
      get { return typeof(T); }
    }

    #region Index
    
    public T this [Position pos]
    {
      get
      { 
        if (Outside(pos))
          throw new ArgumentOutOfRangeException(pos + " not in layer");
        return container [pos.x, pos.y]; 
      }
      private set
      {
        if (Outside(pos))
          throw new ArgumentOutOfRangeException(pos + " not in layer");
        container [pos.x, pos.y] = value;
      }
    }
    
    #endregion

    #region Basic Methods

    public void Put(GameObject go, Position pos)
    {
      if (!Empty(pos))
        throw new ArgumentException(pos + " already occupied");
      T item = go.GetComponent<T>();
      if (item == null) 
        throw new ArgumentException(go.name + " doesn't contain a Component of Type " + ItemType);
      this [pos] = item;
      item.Position = pos;
      item.Layer = this;
      go.transform.parent = SceneNode.transform;
    }

    public GameObject Remove(Position pos)
    {
      if (Empty(pos))
        throw new ArgumentException(pos + " is empty");
      T item = this [pos];
      this [pos] = null;
      item.Layer = null;
      item.Position = null;
      return item.gameObject;
    }

    public void Purge(Position pos)
    {
      GameObject.Destroy(Remove(pos));
    }

    public void Move(Position from, Position to)
    {
      Put(Remove(from), to);
    }

    public bool Empty(Position pos)
    {
      return this [pos] == null;
    }

    public void Clear()
    {
      foreach (T item in Items)
      {
        Purge(item.Position);
      }
    }

    #endregion

    #region Bound Checking

    public bool Inside(Position pos)
    {
      return pos.x >= 0
        && pos.x < Width
        && pos.y >= 0
        && pos.y < Height;
    }

    public bool Outside(Position pos)
    {
      return pos.x < 0
        || pos.x >= Width
        || pos.y < 0
        || pos.y >= Height;
    }

    #endregion

    #region Test Functions

    public IEnumerable<T> Where(Func<T,bool> test)
    {
      foreach (T item in Items)
      {
        if (test(item))
        {
          yield return item;
        }
      }
    }

    public IEnumerable<Position> Where(Func<Position,bool> test)
    {
      for (int x = 0, width = Width; x < width; x++)
      {
        for (int y = 0, height = Height; y < height; y++)
        {
          Position pos = new Position(x, y);
          if (test(pos))
          {
            yield return pos;
          }
        }
      }
    }

    #endregion

    #region Utilities

    public Position Mirror(Position pos)
    {
      return new Position(Width - pos.x - 1, Height - pos.y - 1);
    }

    public Position[] Ray(Position pos, Direction direction)
    {
      List<Position> list = new List<Position>();
      while (Inside(pos+=direction))
      {
        list.Add(pos);
      }
      return list.ToArray();
    }

    public Position[] ForX(int x)
    {
      return Ray(new Position(x, -1), Direction.Up);
    }

    public Position[] ForY(int y)
    {
      return Ray(new Position(-1, y), Direction.Right);
    }

    #endregion

  }
}