using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("survey")]
public record GcloudSurveyOptions : GcloudOptions;