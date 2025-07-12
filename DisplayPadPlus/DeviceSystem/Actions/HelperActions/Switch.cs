using DisplayPadPlus.DeviceSystem.Visuals;
using System.CodeDom;
using System.Drawing;

namespace DisplayPadPlus.DeviceSystem.Actions.HelperActions;
public class Switch : ASimpleAction
{
    public readonly ASimpleAction DectivationAction;
    public readonly ASimpleAction ActivationAction;

    public bool Active;

    public Switch(ASimpleAction dectivationAction, ASimpleAction activationAction, bool active = false)
    {
        Active = active;
        DectivationAction = dectivationAction;
        ActivationAction = activationAction;

        MakeAnimations(out IconAnimation whenDisabled, out IconAnimation whenActivated);
        WhenActivated = whenActivated;
        WhenDisabled = whenDisabled;
    }


    public override void InvokeAction()
    {
        Active = !Active;
        if (Active)
        {
            ActivationAction.InvokeAction();
            PlayAnimation(WhenActivated);
        }
        else
        {
            DectivationAction.InvokeAction();
            PlayAnimation(WhenDisabled);
        }
    }


    #region Visual
    readonly IconAnimation WhenActivated;
    readonly IconAnimation WhenDisabled;


    private void MakeAnimations(out IconAnimation whenDisabled, out IconAnimation whenActivated)
    {
        const int frames = 11;
        const int durationMS = 500;
        IconData[] datas = new IconData[frames];
        for (int i = 0; i < frames; i++)
        {
            datas[i] = AddOntoBackGround(DrawSwitch((float)i / frames - 1));
        }

        IconData[] activ = new IconData[frames - 1];
        for (int i = 0; i < activ.Length; i++)
            activ[i] = datas[i + 1];
        
        whenActivated = new(activ, TimeSpan.FromMilliseconds(durationMS));

        IconData[] disab = new IconData[frames - 1];
        for (int i = 0; i < disab.Length; i++)
            disab[i] = datas[datas.Length - 1 - i];

        whenDisabled = new(disab, TimeSpan.FromMilliseconds(durationMS));
    }


    private Bitmap DrawSwitch(float state)
    {
        Bitmap bmp = new(DisplayConsts.IconSize, DisplayConsts.IconSize);
        DrawInner(bmp, state);
        DrawRamen(bmp);
        DrawSlider(bmp, state);
        return bmp;
    }

    private void DrawInner(Bitmap bmp0, float state)
    {
        //throw new NotImplementedException();
    }

    private void DrawRamen(Bitmap bmp1)
    {
        //throw new NotImplementedException();
    }

    private void DrawSlider(Bitmap bmp2, float state)
    {
        //throw new NotImplementedException();
    }

    #endregion
}
