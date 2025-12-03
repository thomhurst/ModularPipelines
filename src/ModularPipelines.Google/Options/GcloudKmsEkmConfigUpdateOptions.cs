using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-config", "update")]
public record GcloudKmsEkmConfigUpdateOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--default-ekm-connection")]
    public string? DefaultEkmConnection { get; set; }
}