using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "set-iam-policy")]
public record GcloudStorageBucketsSetIamPolicyOptions(
[property: PositionalArgument] string Urls,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}