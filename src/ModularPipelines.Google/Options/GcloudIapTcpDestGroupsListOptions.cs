using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "list")]
public record GcloudIapTcpDestGroupsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}