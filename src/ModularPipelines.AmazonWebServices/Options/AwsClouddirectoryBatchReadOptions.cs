using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "batch-read")]
public record AwsClouddirectoryBatchReadOptions(
[property: CommandSwitch("--directory-arn")] string DirectoryArn,
[property: CommandSwitch("--operations")] string[] Operations
) : AwsOptions
{
    [CommandSwitch("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}