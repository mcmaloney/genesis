using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class AddButton : Button
    {
        public GameObject dialogObject;

        public void OnBClick()
        {
            activateDialogObject();
        }

        public void activateDialogObject()
        {
            transform.parent.gameObject.SetActive(false);
            dialogObject.SetActive(true);
        }
    }
}

