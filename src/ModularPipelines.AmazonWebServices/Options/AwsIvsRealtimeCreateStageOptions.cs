using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs-realtime", "create-stage")]
public record AwsIvsRealtimeCreateStageOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--participant-token-configurations")]
    public string[]? ParticipantTokenConfigurations { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}