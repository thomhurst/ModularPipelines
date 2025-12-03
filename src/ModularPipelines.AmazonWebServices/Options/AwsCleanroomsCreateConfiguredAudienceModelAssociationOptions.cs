using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-configured-audience-model-association")]
public record AwsCleanroomsCreateConfiguredAudienceModelAssociationOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--configured-audience-model-arn")] string ConfiguredAudienceModelArn,
[property: CliOption("--configured-audience-model-association-name")] string ConfiguredAudienceModelAssociationName
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}