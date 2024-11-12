using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CameraPlayPauseBehaviour : MonoBehaviour
{
    private Toggle _toggle;
    // Start is called before the first frame update

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(ToggleListener);
    }

    private void ToggleListener(bool value)
    {
        VuforiaRenderer.Instance.Pause(value);
    }
}
