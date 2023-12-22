using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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