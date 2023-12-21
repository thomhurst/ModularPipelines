using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint")]
public class AzCdnEndpointWaf
{
    public AzCdnEndpointWaf(
        AzCdnEndpointWafPolicy policy
    )
    {
        Policy = policy;
    }

    public AzCdnEndpointWafPolicy Policy { get; }
}