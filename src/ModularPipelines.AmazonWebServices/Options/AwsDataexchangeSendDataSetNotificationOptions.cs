using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "send-data-set-notification")]
public record AwsDataexchangeSendDataSetNotificationOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--details")]
    public string? Details { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}