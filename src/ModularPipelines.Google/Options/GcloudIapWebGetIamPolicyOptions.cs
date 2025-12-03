using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "web", "get-iam-policy")]
public record GcloudIapWebGetIamPolicyOptions : GcloudOptions
{
    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}