using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn")]
public class AzCdnWaf
{
    public AzCdnWaf(
        AzCdnWafPolicy policy
    )
    {
        Policy = policy;
    }

    public AzCdnWafPolicy Policy { get; }
}

