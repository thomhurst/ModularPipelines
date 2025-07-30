namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the position of a positional argument relative to command switches.
/// </summary>
public enum Position
{
    /// <summary>
    /// The argument should appear before any command switches.
    /// </summary>
    BeforeSwitches,
    
    /// <summary>
    /// The argument should appear after all command switches.
    /// </summary>
    AfterSwitches,
}