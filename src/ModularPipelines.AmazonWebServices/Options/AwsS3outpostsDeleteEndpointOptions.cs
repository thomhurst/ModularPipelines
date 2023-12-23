using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3outposts", "delete-endpoint")]
public record AwsS3outpostsDeleteEndpointOptions(
[property: CommandSwitch("--endpoint-id")] string EndpointId,
[property: CommandSwitch("--outpost-id")] string OutpostId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}