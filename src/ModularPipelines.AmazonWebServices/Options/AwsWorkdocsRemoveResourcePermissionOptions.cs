using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "remove-resource-permission")]
public record AwsWorkdocsRemoveResourcePermissionOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--principal-id")] string PrincipalId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--principal-type")]
    public string? PrincipalType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}