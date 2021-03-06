﻿using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;


namespace Medusa
{

    public class TurnActions
    {

        public TurnActions(List<Action> actions)
        {
            ActionList = actions;
			ActionJSON = GenerateJSON(actions);
        }


        public TurnActions(JSONNode json)
        {
            ActionList = ParseJSON(json);
            ActionJSON = json;
        }


		private JSONClass GenerateJSON(List<Action> actions)
        {
			if (actions.Count == 0)
				return null;

	        JSONClass json = new JSONClass();
	        int c = 0, count = 0;

	        foreach (var action in actions)
	        {
				json["actions"][c]["charpos"] = action.CharacterPos.ToJSON();
		        json["actions"][c]["skillname"] = action.SkillName;

		        foreach (var pos in action.TargetPositions)
		        {
					json["actions"][c]["targetpos"][count] = pos.ToJSON();
			        count++;
		        }

		        c++;
	        }

			return json;
        }


		private List<Action> ParseJSON(JSONNode json)
		{
			Debug.Log("At ParseJSON: " + json.ToString());

			return (from JSONNode action in json["actions"].AsArray 
					let charPos = Position.FromJSON(action["charpos"]) 
					let skillName = action["skillname"].Value 
					let positions = (from JSONNode pos in action["targetpos"].AsArray 
									 select Position.FromJSON(pos)).ToList() 

					select new Action(charPos, skillName, positions)).ToList();
	    }


        public List<Action> ActionList { get; private set; }

        public JSONNode ActionJSON { get; private set; }

    }

}
