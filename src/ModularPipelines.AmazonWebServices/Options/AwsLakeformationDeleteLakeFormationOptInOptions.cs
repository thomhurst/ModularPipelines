using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "delete-lake-formation-opt-in")]
public record AwsLakeformationDeleteLakeFormationOptInOptions(
[property: CommandSwitch("--principal")] string Principal,
[property: CommandSwitch("--resource")] string Resource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}