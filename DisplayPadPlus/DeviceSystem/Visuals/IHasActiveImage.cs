﻿using DisplayPadPlus.DeviceSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayPadPlus.DeviceSystem.Visuals;
public interface IHasActiveImage : IHasDefaultImage
{
    Bitmap GetActiveImage();
}
