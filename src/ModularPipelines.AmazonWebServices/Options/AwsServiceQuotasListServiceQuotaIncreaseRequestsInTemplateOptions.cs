using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "list-service-quota-increase-requests-in-template")]
public record AwsServiceQuotasListServiceQuotaIncreaseRequestsInTemplateOptions : AwsOptions
{
    [CommandSwitch("--service-code")]
    public string? ServiceCode { get; set; }

    [CommandSwitch("--aws-region")]
    public string? AwsRegion { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}