using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "get-access-token")]
public record AwsLicenseManagerGetAccessTokenOptions(
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--token-properties")]
    public string[]? TokenProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}