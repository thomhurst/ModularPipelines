using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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