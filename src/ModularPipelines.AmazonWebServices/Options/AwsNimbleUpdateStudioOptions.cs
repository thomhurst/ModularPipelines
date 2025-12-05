using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "update-studio")]
public record AwsNimbleUpdateStudioOptions(
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--admin-role-arn")]
    public string? AdminRoleArn { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--user-role-arn")]
    public string? UserRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}