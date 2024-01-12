using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-delegated-prefixes", "delegated-sub-prefixes", "delete")]
public record GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixesDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [BooleanCommandSwitch("--global-public-delegated-prefix")]
    public bool? GlobalPublicDelegatedPrefix { get; set; }

    [CommandSwitch("--public-delegated-prefix-region")]
    public string? PublicDelegatedPrefixRegion { get; set; }
}