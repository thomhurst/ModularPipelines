using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-app")]
public record AwsSagemakerCreateAppOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--app-type")] string AppType,
[property: CliOption("--app-name")] string AppName
) : AwsOptions
{
    [CliOption("--user-profile-name")]
    public string? UserProfileName { get; set; }

    [CliOption("--space-name")]
    public string? SpaceName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--resource-spec")]
    public string? ResourceSpec { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}