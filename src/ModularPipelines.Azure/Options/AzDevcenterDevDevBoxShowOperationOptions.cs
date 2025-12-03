using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "dev", "dev-box", "show-operation")]
public record AzDevcenterDevDevBoxShowOperationOptions(
[property: CliOption("--dev-box-name")] string DevBoxName,
[property: CliOption("--operation-id")] string OperationId,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}