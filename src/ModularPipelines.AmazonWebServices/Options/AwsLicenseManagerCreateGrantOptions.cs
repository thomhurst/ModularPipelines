using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-grant")]
public record AwsLicenseManagerCreateGrantOptions(
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--grant-name")] string GrantName,
[property: CommandSwitch("--license-arn")] string LicenseArn,
[property: CommandSwitch("--principals")] string[] Principals,
[property: CommandSwitch("--home-region")] string HomeRegion,
[property: CommandSwitch("--allowed-operations")] string[] AllowedOperations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}