using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "batch-get-vpc-endpoint")]
public record AwsOpensearchserverlessBatchGetVpcEndpointOptions(
[property: CommandSwitch("--ids")] string[] Ids
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}