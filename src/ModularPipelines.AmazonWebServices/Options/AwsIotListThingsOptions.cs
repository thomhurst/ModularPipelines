using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-things")]
public record AwsIotListThingsOptions : AwsOptions
{
    [CommandSwitch("--attribute-name")]
    public string? AttributeName { get; set; }

    [CommandSwitch("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CommandSwitch("--thing-type-name")]
    public string? ThingTypeName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}