using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "create-listener")]
public record AwsVpcLatticeCreateListenerOptions(
[property: CommandSwitch("--default-action")] string DefaultAction,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}