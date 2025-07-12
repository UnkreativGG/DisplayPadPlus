namespace DisplayPadPlus.DeviceSystem.Actions;
public abstract class ASimpleAction : AAction
{
    private DateTime? TimePressed = null;
    
    
    public abstract void InvokeAction();

    
    public override void Down()
    {
        TimePressed = DateTime.Now;
    }

    public override void Up()
    {
        if (DateTime.Now - TimePressed < TimeSpan.FromMilliseconds(333))
            InvokeAction();

        TimePressed = null;
    }
}
