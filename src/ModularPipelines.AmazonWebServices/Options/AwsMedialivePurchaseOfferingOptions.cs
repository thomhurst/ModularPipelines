using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "purchase-offering")]
public record AwsMedialivePurchaseOfferingOptions(
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--offering-id")] string OfferingId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--renewal-settings")]
    public string? RenewalSettings { get; set; }

    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}