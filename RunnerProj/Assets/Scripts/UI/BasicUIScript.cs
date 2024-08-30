using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Runner.BasecUI
{

    public class BasicUIScript : MonoBehaviour
    {

        //protected Canvas _canvasUI;
        // Start is called before the first frame update
        void Start()
        {
            //_canvasUI = GetComponent<Canvas>();
        }

        public virtual void HideCanvas()
        {
            if (GetComponent<Canvas>())
                GetComponent<Canvas>().enabled = false;
        }

        public virtual void ShowCanvas()
        {
            if (GetComponent<Canvas>())
                GetComponent<Canvas>().enabled = true;
            // Debug.Log("Canwas is "+_canvasUI.gameObject.name);

        }

    }

}