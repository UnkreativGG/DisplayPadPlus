using DisplayPadPlus.DeviceSystem.Actions;
using KeySimmulator.Macros.MacroActions;
using System.Drawing;

namespace KeySimmulator.Macros;
internal class Macro : ASimpleAction
{
    private readonly IMacroAction[] Actions;

    private readonly int timesToPlay;
    private readonly bool finishWhenDeactivated;



    public Macro(MacroBuilder builder)
    {
        timesToPlay = builder.TimesToPlay;
        finishWhenDeactivated = builder.FinishWhenDeactivated;

        List<IMacroAction> macroActions = [];

        int length = builder.MacroActions.Count;

        List<User32_SendInput.Input> inputActions = [];
        for (int i = 0; i < length; i++)
        {
            IMacroAction action = builder.MacroActions[i];

            if (action is InputAction inputAction2)
            {
                for (int j = 0; j < inputAction2.Input.Length; j++)
                    inputActions.Add(inputAction2.Input[j]);
            }
            else
            {
                if (inputActions.Count > 0)
                {
                    macroActions.Add(new InputAction([.. inputActions]));
                    inputActions.Clear();
                }
                macroActions.Add(action);
            }
        }

        if (inputActions.Count > 0)
            macroActions.Add(new InputAction([.. inputActions]));

        Actions = [.. macroActions];
    }



    public override void InvokeAction()
    {
        if (isActive)
            return;
        isActive = true;
        new Thread(ExecuteMacro).Start();
    }



    public void CancelAction()
    {
        isActive = false;
    }

    private bool isActive;
    public bool IsActive => isActive;

    private void ExecuteMacro()
    {
        int timesplayed = 0;

        while (timesplayed < timesToPlay || timesToPlay == -1)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                if (!IsActive)
                {
                    isActive = false;
                    return;
                }
                Actions[i].Execute();
            }
            timesplayed++;
        }
        isActive = false;
    }



    public Bitmap GetActiveImage()
    {
        throw new NotImplementedException();
        return new Bitmap(0, 0);
    }

    public Bitmap GetImage()
    {
        throw new NotImplementedException();
        return new Bitmap(0, 0);
    }
}
