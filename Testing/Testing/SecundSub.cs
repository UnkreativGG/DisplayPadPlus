using DisplayPadPlus.DeviceSystem;
using DisplayPadPlus.DeviceSystem.Actions.DisplayPadControllerActions;

namespace Testing.Layers;
internal class SecundSub : Layer
{
    public override void Config()
    {
        Background = new(@"---");

        CreateIcon(1)
            .OverwriteImageWithPath(@"---")
            /*.SetAction(IAction.ActionType.ShortPress, VirtualKeyCode.VK_X)*/;

        CreateIcon(11)
            .SetAsGoBack();
    }
}
