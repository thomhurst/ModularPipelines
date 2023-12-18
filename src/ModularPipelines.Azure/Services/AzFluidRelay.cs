using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzFluidRelay
{
    public AzFluidRelay(
        AzFluidRelayContainer container,
        AzFluidRelayServer server
    )
    {
        Container = container;
        Server = server;
    }

    public AzFluidRelayContainer Container { get; }

    public AzFluidRelayServer Server { get; }
}