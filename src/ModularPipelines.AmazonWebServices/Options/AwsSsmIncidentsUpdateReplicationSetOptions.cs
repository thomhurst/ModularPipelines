using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "update-replication-set")]
public record AwsSsmIncidentsUpdateReplicationSetOptions(
[property: CliOption("--actions")] string[] Actions,
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}