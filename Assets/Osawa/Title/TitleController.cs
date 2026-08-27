using UnityEngine;
using UnityEngine.InputSystem;

public class TitleController : MonoBehaviour
{
    InputAction _submitInput = null;

    private void Start()
    {
        _submitInput = InputSystem.actions.FindAction("Submit");

        SoundManager.instance.PlayBGM(0);
    }

    private void Update()
    {
        if (_submitInput.WasPressedThisFrame())
        {
            StageSceneManager.instance.LoadStage(0);
            SoundManager.instance.PlayBGM(1);
        }
    }
}
