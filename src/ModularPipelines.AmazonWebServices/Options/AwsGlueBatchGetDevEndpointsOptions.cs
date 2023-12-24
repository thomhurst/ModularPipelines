using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "batch-get-dev-endpoints")]
public record AwsGlueBatchGetDevEndpointsOptions(
[property: CommandSwitch("--dev-endpoint-names")] string[] DevEndpointNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}