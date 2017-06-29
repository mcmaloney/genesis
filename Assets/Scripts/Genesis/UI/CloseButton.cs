using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class CloseButton : Button
    {
        public GameObject transitionObject;

        public void OnClick()
        {
            transform.parent.gameObject.SetActive(false);
            transitionObject.SetActive(true);
        }
    }
}

