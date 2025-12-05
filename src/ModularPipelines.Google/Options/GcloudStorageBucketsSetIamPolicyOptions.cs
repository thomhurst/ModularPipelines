using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "set-iam-policy")]
public record GcloudStorageBucketsSetIamPolicyOptions(
[property: CliArgument] string Urls,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }
}