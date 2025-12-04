using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment", "show-operation")]
public record AzDevcenterDevEnvironmentShowOperationOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
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