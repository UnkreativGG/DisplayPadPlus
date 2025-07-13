using DisplayPadPlus;
using DisplayPadPlus.DeviceSystem.Visuals;
using Testing.Layers;

DisplayPadManager.GlobalDefaultBackground = new Background(@"---", true);
DisplayPadManager.AddDevice<Home>();
DisplayPadManager.Finish();
