using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "update-datasource-packages")]
public record AwsDetectiveUpdateDatasourcePackagesOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn,
[property: CommandSwitch("--datasource-packages")] string[] DatasourcePackages
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}