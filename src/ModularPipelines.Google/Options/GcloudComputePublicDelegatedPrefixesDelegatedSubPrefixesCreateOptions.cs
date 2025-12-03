using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-delegated-prefixes", "delegated-sub-prefixes", "create")]
public record GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [CliFlag("--create-addresses")]
    public bool? CreateAddresses { get; set; }

    [CliOption("--delegatee-project")]
    public string? DelegateeProject { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }

    [CliFlag("--global-public-delegated-prefix")]
    public bool? GlobalPublicDelegatedPrefix { get; set; }

    [CliOption("--public-delegated-prefix-region")]
    public string? PublicDelegatedPrefixRegion { get; set; }
}