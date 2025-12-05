using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "import-crl")]
public record AwsRolesanywhereImportCrlOptions(
[property: CliOption("--crl-data")] string CrlData,
[property: CliOption("--name")] string Name,
[property: CliOption("--trust-anchor-arn")] string TrustAnchorArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}