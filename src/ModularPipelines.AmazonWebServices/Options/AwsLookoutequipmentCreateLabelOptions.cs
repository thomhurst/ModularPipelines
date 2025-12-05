using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-label")]
public record AwsLookoutequipmentCreateLabelOptions(
[property: CliOption("--label-group-name")] string LabelGroupName,
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--rating")] string Rating
) : AwsOptions
{
    [CliOption("--fault-code")]
    public string? FaultCode { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--equipment")]
    public string? Equipment { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}