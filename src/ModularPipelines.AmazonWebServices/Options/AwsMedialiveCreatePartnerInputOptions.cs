using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "create-partner-input")]
public record AwsMedialiveCreatePartnerInputOptions(
[property: CommandSwitch("--input-id")] string InputId
) : AwsOptions
{
    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}