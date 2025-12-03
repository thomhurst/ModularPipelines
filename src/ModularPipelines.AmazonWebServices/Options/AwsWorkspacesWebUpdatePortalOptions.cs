using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "update-portal")]
public record AwsWorkspacesWebUpdatePortalOptions(
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}