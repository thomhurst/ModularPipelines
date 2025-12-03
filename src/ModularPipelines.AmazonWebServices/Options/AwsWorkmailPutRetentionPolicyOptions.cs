using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-retention-policy")]
public record AwsWorkmailPutRetentionPolicyOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--folder-configurations")] string[] FolderConfigurations
) : AwsOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}