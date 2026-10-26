using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class N_HoldPress : MonoBehaviour
{
    [SerializeField] private InputAction _inputAction;

    private void OnEnable()
    {
        // performedコールバックを登録
        _inputAction.performed += OnPerformed;

        // 入力を受け取るにはInputActionを有効化する必要がある
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        // InputActionを無効化
        _inputAction.Disable();

        // performedコールバックの登録を解除
        _inputAction.performed -= OnPerformed;
    }

    // InputActionのperformedイベント
    private void OnPerformed(InputAction.CallbackContext ctx)
    {
        // 短押しか長押しかの判別
        switch (ctx.interaction)
        {
            case TapInteraction:
                // TapInteractionの場合は短押し
                //Debug.Log("短押しされたよ！");
                break;

            case HoldInteraction:
                // HoldInteractionの場合は長押し
               // Debug.Log("長押しされたよ！");
                break;
        }
    }

    
}
