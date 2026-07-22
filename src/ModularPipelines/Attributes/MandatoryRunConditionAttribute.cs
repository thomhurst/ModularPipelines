namespace ModularPipelines.Attributes;

/// <summary>
/// Abstract base class for run condition attributes that are mandatory for execution. 
/// If this condition fails, the module will be skipped.
/// </summary>
[Obsolete("Use SkipIfAttribute<T>, RunIfAllAttribute<T>, RunIfAnyAttribute<T>, or ModuleConfiguration.Create().WithSkipWhen(...) instead.")]
#pragma warning disable CS0618 // The obsolete base type is retained solely for source compatibility.
public abstract class MandatoryRunConditionAttribute : RunConditionAttribute;
#pragma warning restore CS0618
