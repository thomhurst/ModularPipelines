using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzTsi
{
    public AzTsi(
        AzTsiAccessPolicy accessPolicy,
        AzTsiEnvironment environment,
        AzTsiEventSource eventSource,
        AzTsiReferenceDataSet referenceDataSet
    )
    {
        AccessPolicy = accessPolicy;
        Environment = environment;
        EventSource = eventSource;
        ReferenceDataSet = referenceDataSet;
    }

    public AzTsiAccessPolicy AccessPolicy { get; }

    public AzTsiEnvironment Environment { get; }

    public AzTsiEventSource EventSource { get; }

    public AzTsiReferenceDataSet ReferenceDataSet { get; }
}