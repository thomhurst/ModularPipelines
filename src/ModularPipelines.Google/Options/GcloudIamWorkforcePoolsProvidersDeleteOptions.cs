using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "delete")]
public record GcloudIamWorkforcePoolsProvidersDeleteOptions(
[property: CliArgument] string Provider,
[property: CliArgument] string Location,
[property: CliArgument] string WorkforcePool
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}