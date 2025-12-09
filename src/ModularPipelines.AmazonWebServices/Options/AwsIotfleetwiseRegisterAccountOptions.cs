using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "register-account")]
public record AwsIotfleetwiseRegisterAccountOptions : AwsOptions
{
    [CliOption("--timestream-resources")]
    public string? TimestreamResources { get; set; }

    [CliOption("--iam-resources")]
    public string? IamResources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}