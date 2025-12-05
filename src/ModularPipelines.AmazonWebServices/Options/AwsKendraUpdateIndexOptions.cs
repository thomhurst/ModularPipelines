using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "update-index")]
public record AwsKendraUpdateIndexOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--document-metadata-configuration-updates")]
    public string[]? DocumentMetadataConfigurationUpdates { get; set; }

    [CliOption("--capacity-units")]
    public string? CapacityUnits { get; set; }

    [CliOption("--user-token-configurations")]
    public string[]? UserTokenConfigurations { get; set; }

    [CliOption("--user-context-policy")]
    public string? UserContextPolicy { get; set; }

    [CliOption("--user-group-resolution-configuration")]
    public string? UserGroupResolutionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}