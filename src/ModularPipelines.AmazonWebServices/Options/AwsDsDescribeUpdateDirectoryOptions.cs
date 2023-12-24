using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "describe-update-directory")]
public record AwsDsDescribeUpdateDirectoryOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--update-type")] string UpdateType
) : AwsOptions
{
    [CommandSwitch("--region-name")]
    public string? RegionName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}