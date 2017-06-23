using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class ListItem : MonoBehaviour
    {
        public GameObject labelText;
        public string labelTextValue;
        public Material defaultMaterial;
        public Material hoverMaterial;

        private Renderer _renderer;

        public void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = defaultMaterial;
            labelText.GetComponent<TextMesh>().text = labelTextValue;
        }

        public void OnMouseOver()
        {
            switchMaterial(hoverMaterial);
        }

        public void OnMouseExit()
        {
            switchMaterial(defaultMaterial);
        }

        public void OnBClick()
        {
            Debug.Log("List Item Clicked");
        }

        private void switchMaterial(Material mat)
        {
            _renderer.material = mat;
        }
    }

}
