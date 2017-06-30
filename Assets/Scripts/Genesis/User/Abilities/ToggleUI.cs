using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Genesis.UI;
using Genesis.User;

public class ToggleUI : MonoBehaviour {
    public GameObject UI;
    public GameObject Player;
    public OVRInput.Controller Controller;
    public OVRInput.Button ToggleButton;

    private UIController _UI;
    private Player _Player;

    private void Start()
    {
        _UI = UI.GetComponent<UIController>();
        _Player = Player.GetComponent<Player>();
    }

    void Update () {
        if (OVRInput.GetDown(ToggleButton, Controller))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (!UI.activeSelf)
        {
            _UI.SetInitialState(Player.transform.position + _Player.UIOffset);
        }
        else
        {
            _UI.Close();
        }
    }
}
