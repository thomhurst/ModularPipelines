using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "dev-box", "show-operation")]
public record AzDevcenterDevDevBoxShowOperationOptions(
[property: CommandSwitch("--dev-box-name")] string DevBoxName,
[property: CommandSwitch("--operation-id")] string OperationId,
[property: CommandSwitch("--project")] string Project
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}