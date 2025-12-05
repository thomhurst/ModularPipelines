using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-label-group")]
public record AwsLookoutequipmentCreateLabelGroupOptions(
[property: CliOption("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CliOption("--fault-codes")]
    public string[]? FaultCodes { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}