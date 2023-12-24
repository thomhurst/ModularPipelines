using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "delete-ops-metadata")]
public record AwsSsmDeleteOpsMetadataOptions(
[property: CommandSwitch("--ops-metadata-arn")] string OpsMetadataArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}