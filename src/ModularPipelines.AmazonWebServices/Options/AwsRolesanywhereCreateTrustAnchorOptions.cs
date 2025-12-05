using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "create-trust-anchor")]
public record AwsRolesanywhereCreateTrustAnchorOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source")] string Source
) : AwsOptions
{
    [CliOption("--notification-settings")]
    public string[]? NotificationSettings { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}