using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-token")]
public record AwsLicenseManagerCreateTokenOptions(
[property: CommandSwitch("--license-arn")] string LicenseArn,
[property: CommandSwitch("--client-token")] string ClientToken
) : AwsOptions
{
    [CommandSwitch("--role-arns")]
    public string[]? RoleArns { get; set; }

    [CommandSwitch("--expiration-in-days")]
    public int? ExpirationInDays { get; set; }

    [CommandSwitch("--token-properties")]
    public string[]? TokenProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}