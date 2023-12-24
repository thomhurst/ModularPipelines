using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "create-ip-set")]
public record AwsGuarddutyCreateIpSetOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--location")] string Location
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}