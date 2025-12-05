using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-configured-table-association")]
public record AwsCleanroomsCreateConfiguredTableAssociationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}