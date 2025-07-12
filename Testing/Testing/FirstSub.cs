using DisplayPadPlus.DeviceSystem;
using DisplayPadPlus.DeviceSystem.Actions.DisplayPadControllerActions;

namespace Testing.Layers;

internal class FirstSub : Layer
{
    public override void Config()
    {
        CreateIcon(5)
            .SetAsLayer<FirstSubSub>();

        CreateIcon(9)
            .SetAsGoBack();
    }
}
