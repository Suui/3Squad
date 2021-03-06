﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Medusa
{

    public class Skill : MonoBehaviour
    {

		public int ActionPointCost { get; set; }

        public static Vector3 FirstPos      = new Vector3(0.1f, 0.1f, 0.0f);
        public static Vector3 SecondPos     = new Vector3(0.3f, 0.1f, 0.0f);
        public static Vector3 ThirdPos      = new Vector3(0.5f, 0.1f, 0.0f);


        public virtual void ShowUpSkill()
        {
            
        }


        public virtual void Setup()
        {
            
        }


        public virtual bool Click(Position position)
        {
            return true;
        }


        public virtual void Clear()
        {
            
        }


        public virtual List<Position> Confirm()
        {
	        throw new NotImplementedException();
        }


	    public virtual string GetSkillType()
	    {
		    return "";
        }

    }

}
