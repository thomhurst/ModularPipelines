namespace ModularPipelines.Distributed.Matrix;

/// <summary>
/// Runtime metadata for an expanded matrix module instance.
/// </summary>
internal record MatrixModuleInstance(
    Type OriginalType,
    string TargetValue,
    string InstanceName,
    string CapabilityName
);
