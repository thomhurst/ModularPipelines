using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-delegated-prefixes", "delegated-sub-prefixes", "delete")]
public record GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixesDeleteOptions(
[property: CliArgument] string Name,
[property: CliOption("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [CliFlag("--global-public-delegated-prefix")]
    public bool? GlobalPublicDelegatedPrefix { get; set; }

    [CliOption("--public-delegated-prefix-region")]
    public string? PublicDelegatedPrefixRegion { get; set; }
}