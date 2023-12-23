using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "describe-patch-properties")]
public record AwsSsmDescribePatchPropertiesOptions(
[property: CommandSwitch("--operating-system")] string OperatingSystem,
[property: CommandSwitch("--property")] string Property
) : AwsOptions
{
    [CommandSwitch("--patch-set")]
    public string? PatchSet { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}