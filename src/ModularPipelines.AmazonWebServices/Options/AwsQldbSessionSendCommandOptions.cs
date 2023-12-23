using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb-session", "send-command")]
public record AwsQldbSessionSendCommandOptions : AwsOptions
{
    [CommandSwitch("--session-token")]
    public string? SessionToken { get; set; }

    [CommandSwitch("--start-session")]
    public string? StartSession { get; set; }

    [CommandSwitch("--start-transaction")]
    public string? StartTransaction { get; set; }

    [CommandSwitch("--end-session")]
    public string? EndSession { get; set; }

    [CommandSwitch("--commit-transaction")]
    public string? CommitTransaction { get; set; }

    [CommandSwitch("--abort-transaction")]
    public string? AbortTransaction { get; set; }

    [CommandSwitch("--execute-statement")]
    public string? ExecuteStatement { get; set; }

    [CommandSwitch("--fetch-page")]
    public string? FetchPage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}