using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "update-crl")]
public record AwsRolesanywhereUpdateCrlOptions(
[property: CliOption("--crl-id")] string CrlId
) : AwsOptions
{
    [CliOption("--crl-data")]
    public string? CrlData { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}