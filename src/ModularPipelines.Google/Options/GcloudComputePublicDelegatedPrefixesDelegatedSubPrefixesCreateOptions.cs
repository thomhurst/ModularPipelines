using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-delegated-prefixes", "delegated-sub-prefixes", "create")]
public record GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [BooleanCommandSwitch("--create-addresses")]
    public bool? CreateAddresses { get; set; }

    [CommandSwitch("--delegatee-project")]
    public string? DelegateeProject { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--range")]
    public string? Range { get; set; }

    [BooleanCommandSwitch("--global-public-delegated-prefix")]
    public bool? GlobalPublicDelegatedPrefix { get; set; }

    [CommandSwitch("--public-delegated-prefix-region")]
    public string? PublicDelegatedPrefixRegion { get; set; }
}