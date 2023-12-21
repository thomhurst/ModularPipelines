using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzFootprint
{
    public AzFootprint(
        AzFootprintExperiment experiment,
        AzFootprintMeasurementEndpoint measurementEndpoint,
        AzFootprintMeasurementEndpointCondition measurementEndpointCondition,
        AzFootprintProfile profile
    )
    {
        Experiment = experiment;
        MeasurementEndpoint = measurementEndpoint;
        MeasurementEndpointCondition = measurementEndpointCondition;
        Profile = profile;
    }

    public AzFootprintExperiment Experiment { get; }

    public AzFootprintMeasurementEndpoint MeasurementEndpoint { get; }

    public AzFootprintMeasurementEndpointCondition MeasurementEndpointCondition { get; }

    public AzFootprintProfile Profile { get; }
}