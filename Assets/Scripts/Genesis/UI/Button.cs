using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class Button : MonoBehaviour
    {
        public Material defaultMaterial;
        public Material hoverMaterial;

        private Renderer _renderer;

        void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = defaultMaterial;
        }

        public void OnMouseOver()
        {
            switchMaterial(hoverMaterial);
        }

        public void OnMouseExit()
        {
            switchMaterial(defaultMaterial);
        }

        private void switchMaterial(Material mat)
        {
            _renderer.material = mat;
        }
    }

}
