namespace ModularPipelines.Attributes;

/// <summary>
/// Abstract base class for run condition attributes that are mandatory for execution. 
/// If this condition fails, the module will be skipped.
/// </summary>
public abstract class MandatoryRunConditionAttribute : RunConditionAttribute;
