using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "create-label")]
public record AwsLookoutequipmentCreateLabelOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName,
[property: CommandSwitch("--start-time")] long StartTime,
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--rating")] string Rating
) : AwsOptions
{
    [CommandSwitch("--fault-code")]
    public string? FaultCode { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--equipment")]
    public string? Equipment { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}