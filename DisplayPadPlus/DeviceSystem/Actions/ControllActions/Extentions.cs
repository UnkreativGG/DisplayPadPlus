using DisplayPadPlus.DeviceSystem.Actions.ControllActions;

namespace DisplayPadPlus.DeviceSystem.Actions.DisplayPadControllerActions
{
    public static class Extentions
    {
        public static DisplayIcon SetAsGoBack(this DisplayIcon displayIcon)
            => displayIcon.SetAction(new GoBack(displayIcon));

        public static DisplayIcon SetAsGoHome(this DisplayIcon displayIcon)
            => displayIcon.SetAction(new GoHome(displayIcon));
    }
}
