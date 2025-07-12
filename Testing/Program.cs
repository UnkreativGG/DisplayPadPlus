using DisplayPadPlus;
using DisplayPadPlus.DeviceSystem.Visuals;
using Testing.Layers;

DisplayPadManager.GlobalDefaultBackground = new Background(@"---", true);
# if true
DisplayPadManager.AddDevice<Home>();
#else
    DisplayPadManager.AddDevice<MC>();
#endif
DisplayPadManager.Finish();
