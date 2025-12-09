using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-grant")]
public record AwsLicenseManagerCreateGrantOptions(
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--grant-name")] string GrantName,
[property: CliOption("--license-arn")] string LicenseArn,
[property: CliOption("--principals")] string[] Principals,
[property: CliOption("--home-region")] string HomeRegion,
[property: CliOption("--allowed-operations")] string[] AllowedOperations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}