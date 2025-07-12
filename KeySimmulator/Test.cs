namespace KeySimmulator;
public class Test
{
    public static void Teast()
    {
        User32_SendInput.Keyboard.KeyboardEventVK vk = User32_SendInput.Keyboard.KeyboardEventVK.VK_F;

        User32_SendInput.Keyboard.KeyboardInput keyboardInput1 = default;
        keyboardInput1.wVk = vk;
        keyboardInput1.dwFlags = false ? 1u : 0u;
        User32_SendInput.Input input1 = default;
        input1.type = User32_SendInput.SendInputEventType.InputKeyboard;
        input1.mkhi.ki = keyboardInput1;


        User32_SendInput.Keyboard.KeyboardInput keyboardInput2 = default;
        keyboardInput2.wVk = vk;
        keyboardInput2.dwFlags = false ? 3u : 2u;
        User32_SendInput.Input input2 = default;
        input2.type = User32_SendInput.SendInputEventType.InputKeyboard;
        input2.mkhi.ki = keyboardInput2;



        User32_SendInput.SendInput([input1, input2, input1, input2]);
    }
}
