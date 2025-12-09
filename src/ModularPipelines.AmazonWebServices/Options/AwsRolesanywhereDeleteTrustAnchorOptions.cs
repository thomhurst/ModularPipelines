using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "delete-trust-anchor")]
public record AwsRolesanywhereDeleteTrustAnchorOptions(
[property: CliOption("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}