using UnityEngine;
using System.Collections;

// Informacion basica

public abstract class BaseInfo : MonoBehaviour{
	
	public string Content{
		get{ return GetInfo(); }
	}	
	protected abstract string GetInfo();
	
	
}
