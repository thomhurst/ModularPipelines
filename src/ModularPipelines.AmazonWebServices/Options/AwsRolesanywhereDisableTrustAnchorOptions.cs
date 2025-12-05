using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "disable-trust-anchor")]
public record AwsRolesanywhereDisableTrustAnchorOptions(
[property: CliOption("--trust-anchor-id")] string TrustAnchorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}