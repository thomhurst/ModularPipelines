using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile", "update-project")]
public record AwsMobileUpdateProjectOptions(
[property: CommandSwitch("--project-id")] string ProjectId
) : AwsOptions
{
    [CommandSwitch("--contents")]
    public string? Contents { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}