namespace ModularPipelines.Distributed.Configuration;

/// <summary>
/// Registered as a service by backends that key shared state by run identifier. Run identifier
/// resolution consults it in addition to <see cref="DistributedOptions.RequireExplicitRunId"/>,
/// so an options binding registered after the backend cannot switch the requirement off.
/// </summary>
internal sealed class ExplicitRunIdRequirement
{
    public static ExplicitRunIdRequirement Instance { get; } = new();

    private ExplicitRunIdRequirement()
    {
    }
}
