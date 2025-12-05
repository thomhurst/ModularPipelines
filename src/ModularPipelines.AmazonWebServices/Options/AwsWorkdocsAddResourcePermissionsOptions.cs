using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "add-resource-permissions")]
public record AwsWorkdocsAddResourcePermissionsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--principals")] string[] Principals
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--notification-options")]
    public string? NotificationOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}