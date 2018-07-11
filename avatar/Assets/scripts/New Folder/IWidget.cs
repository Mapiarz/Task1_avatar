public interface IWidget
{

    /// <summary>
    /// ends the widget in the master, access from master
    /// </summary>
    bool ReadyToSkip { get; set; }

    /// <summary>
    /// choosen exercise, access from master
    /// </summary>
    int Choice { get; set; }

    bool Active { get; set; }

    void Activate();

    void Deactivate();
}
