using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "add-region")]
public record AwsDsAddRegionOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--region-name")] string RegionName,
[property: CommandSwitch("--vpc-settings")] string VpcSettings
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}