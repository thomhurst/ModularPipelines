using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb-session", "send-command")]
public record AwsQldbSessionSendCommandOptions : AwsOptions
{
    [CliOption("--session-token")]
    public string? SessionToken { get; set; }

    [CliOption("--start-session")]
    public string? StartSession { get; set; }

    [CliOption("--start-transaction")]
    public string? StartTransaction { get; set; }

    [CliOption("--end-session")]
    public string? EndSession { get; set; }

    [CliOption("--commit-transaction")]
    public string? CommitTransaction { get; set; }

    [CliOption("--abort-transaction")]
    public string? AbortTransaction { get; set; }

    [CliOption("--execute-statement")]
    public string? ExecuteStatement { get; set; }

    [CliOption("--fetch-page")]
    public string? FetchPage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}