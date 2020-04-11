using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Normal.Realtime.Examples {
  public class ChooseName : MonoBehaviour
  {

      private TMP_InputField _nameInputField;
      private NameSync _nameSync;

      private RealtimeView      _realtimeView;
      private RealtimeTransform _realtimeTransform;

      private void Start() {
          // Get a reference to the color sync component
          _nameSync = GameObject.FindObjectOfType<NameSync>();
          _nameInputField = GameObject.FindObjectOfType<TMP_InputField>();
      }

      private void Awake() {
          _realtimeView      = GetComponent<RealtimeView>();
          _realtimeTransform = GetComponent<RealtimeTransform>();
      }

      private bool isCameraParented = false;

      public void Update()
      {
        if (!_realtimeView.isOwnedLocally)
            return;

        _realtimeTransform.RequestOwnership();

        if(_nameSync == null || _nameInputField == null)
        {
          _nameSync = GameObject.FindObjectOfType<NameSync>();
          _nameInputField = GameObject.FindObjectOfType<TMP_InputField>();
        }
        else
        {
          _nameSync.SetName(_nameInputField.text);
        }
      }
  }
}
