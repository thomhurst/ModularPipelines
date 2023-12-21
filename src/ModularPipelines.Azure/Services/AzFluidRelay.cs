using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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