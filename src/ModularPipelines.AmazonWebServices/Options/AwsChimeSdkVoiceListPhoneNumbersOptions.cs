using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "list-phone-numbers")]
public record AwsChimeSdkVoiceListPhoneNumbersOptions : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--product-type")]
    public string? ProductType { get; set; }

    [CommandSwitch("--filter-name")]
    public string? FilterName { get; set; }

    [CommandSwitch("--filter-value")]
    public string? FilterValue { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}