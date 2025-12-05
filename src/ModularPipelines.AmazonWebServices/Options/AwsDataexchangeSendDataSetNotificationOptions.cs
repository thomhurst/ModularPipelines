using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataexchange", "send-data-set-notification")]
public record AwsDataexchangeSendDataSetNotificationOptions(
[property: CliOption("--data-set-id")] string DataSetId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--details")]
    public string? Details { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}