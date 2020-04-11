using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePicker : MonoBehaviour {
    [SerializeField]
    private string _name;
    private string _previousName;

    private NameSync _nameSync;

    private void Start() {
        // Get a reference to the color sync component
        _nameSync = GetComponent<NameSync>();
    }

    private void Update() {
        // If the color has changed (via the inspector), call SetColor on the color sync component.
        if (_name != _previousName) {
            _nameSync.SetName(_name);
            _previousName = _name;
        }
    }
}
