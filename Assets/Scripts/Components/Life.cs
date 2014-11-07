using UnityEngine;
using System.Collections;

// Se ocupa de mantener la vida

public class Life : BaseInfo {
	
	public int maximumLife = 20;
	public int currentLife = 20;
	
	protected override string GetInfo()
	{
		if(isDead) return "K.O.";
		return "HP: "+currentLife+"/"+maximumLife;
	}
	
	public bool isDead {
		get { return currentLife <= 0; }
	}
	
	public bool Damage(int dmg) 
	{
		currentLife -= dmg;
		
		if(currentLife <= 0) {
			Debug.Log ( gameObject.name + " dies");
			currentLife = 0;
			OnDeath();
			return true;
		}
		
		return false;
	}
	
	protected virtual void OnDeath()
	{
		Destroy(gameObject);
	}
	
}
