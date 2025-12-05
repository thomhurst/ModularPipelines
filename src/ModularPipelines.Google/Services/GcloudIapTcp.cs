using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iap")]
public class GcloudIapTcp
{
    public GcloudIapTcp(
        GcloudIapTcpDestGroups destGroups
    )
    {
        DestGroups = destGroups;
    }

    public GcloudIapTcpDestGroups DestGroups { get; }
}