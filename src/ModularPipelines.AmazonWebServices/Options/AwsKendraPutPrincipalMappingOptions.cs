using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "put-principal-mapping")]
public record AwsKendraPutPrincipalMappingOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--group-members")] string GroupMembers
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--ordering-id")]
    public long? OrderingId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}