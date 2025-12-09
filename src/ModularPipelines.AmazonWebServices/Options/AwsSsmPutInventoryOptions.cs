using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "put-inventory")]
public record AwsSsmPutInventoryOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}