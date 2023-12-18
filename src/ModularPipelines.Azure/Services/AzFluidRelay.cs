using System.Diagnostics.CodeAnalysis;

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