using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-association-status")]
public record AwsSsmUpdateAssociationStatusOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--association-status")] string AssociationStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}