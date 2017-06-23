using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.UI
{
    public class Menu : MonoBehaviour
    {
        public GameObject listItemPrefab;
        public float listItemVerticalMargin;

        public float listItemStackHeight = 0f;

        public void addListItem(string labelValue)
        {
            GameObject listItemObject = Instantiate(listItemPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f));
            listItemObject.transform.parent = transform;
            listItemObject.transform.localScale = new Vector3(0.25f, 1f, 0.06f);
            listItemObject.transform.localPosition = new Vector3(0, listItemVerticalMargin - listItemStackHeight, 0f);
            listItemStackHeight += 0.75f;
            listItemObject.GetComponent<ListItem>().labelTextValue = labelValue;
        }
    }
}
