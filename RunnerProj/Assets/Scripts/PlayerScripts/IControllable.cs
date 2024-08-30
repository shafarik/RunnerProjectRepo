using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner.Controllable
{


    public interface IControllable
    {
        void Move(Vector3 direction);
    }
}