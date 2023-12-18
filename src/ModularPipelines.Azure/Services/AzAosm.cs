using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAosm
{
    public AzAosm(
        AzAosmNfd nfd,
        AzAosmNsd nsd
    )
    {
        Nfd = nfd;
        Nsd = nsd;
    }

    public AzAosmNfd Nfd { get; }

    public AzAosmNsd Nsd { get; }
}