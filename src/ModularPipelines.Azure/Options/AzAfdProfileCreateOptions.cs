using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "profile", "create")]
public record AzAfdProfileCreateOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--origin-response-timeout-seconds")]
    public string? OriginResponseTimeoutSeconds { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}