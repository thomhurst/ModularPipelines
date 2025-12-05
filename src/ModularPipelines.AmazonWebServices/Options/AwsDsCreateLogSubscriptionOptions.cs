using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-log-subscription")]
public record AwsDsCreateLogSubscriptionOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--log-group-name")] string LogGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}