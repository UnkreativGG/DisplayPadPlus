using DisplayPadPlus.DeviceSystem;
using DisplayPadPlus.DeviceSystem.Actions.DisplayPadControllerActions;

namespace Testing.Layers;

internal class FirstSubSub : Layer
{
    public override void Config()
    {
        CreateIcon(3)
            .SetAsGoBack();

        CreateIcon(4)
            .SetAsGoHome();
    }
}
