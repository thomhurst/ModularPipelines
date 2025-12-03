using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-index")]
public record AwsQbusinessCreateIndexOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--display-name")] string DisplayName
) : AwsOptions
{
    [CliOption("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}