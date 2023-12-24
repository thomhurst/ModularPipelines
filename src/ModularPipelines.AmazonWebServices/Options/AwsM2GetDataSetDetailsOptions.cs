using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "get-data-set-details")]
public record AwsM2GetDataSetDetailsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--data-set-name")] string DataSetName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}