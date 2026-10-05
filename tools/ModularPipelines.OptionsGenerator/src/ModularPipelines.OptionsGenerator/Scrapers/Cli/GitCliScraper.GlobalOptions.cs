using System.Text.RegularExpressions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class GitCliScraper
{
    private static readonly Dictionary<string, RootOptionShape> RootOptionShapes = new(StringComparer.Ordinal)
    {
        ["-C"] = new("ChangeDirectories", "string[]?", Phase: CommandLinePhase.EarlyOperand),
        ["-c"] = new("Configuration", "KeyValue[]?", IsSecret: true),
        ["--config-env"] = new("ConfigEnv", "string[]?"),
        ["--exec-path"] = new("ExecPath", "string?"),
        ["--git-dir"] = new("GitDirectory", "string?"),
        ["--work-tree"] = new("WorkTree", "string?"),
        ["--namespace"] = new("Namespace", "string?"),
        ["--bare"] = new("BareRepository"),
        ["--paginate"] = new("Paginate"),
        ["--no-pager"] = new("NoPager"),
        ["--no-replace-objects"] = new("NoReplaceObjects"),
        ["--no-lazy-fetch"] = new("NoLazyFetch"),
        ["--no-optional-locks"] = new("NoOptionalLocks"),
        ["--no-advice"] = new("NoAdvice"),
    };

    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText)
    {
        // Only root usage declares inherited settings; command help reuses short switches.
        var synopsis = helpText.Split("These are common Git commands", StringSplitOptions.None)[0];
        return [.. RootOptionPattern().Matches(synopsis)
            .Select(match => match.Groups[1].Value)
            .Distinct(StringComparer.Ordinal)
            .Where(RootOptionShapes.ContainsKey)
            .Select(name => CreateRootOption(name, RootOptionShapes[name]))];
    }

    private static CliOptionDefinition CreateRootOption(string name, RootOptionShape shape)
    {
        var isFlag = shape.CSharpType == "bool?";
        return new CliOptionDefinition
        {
            SwitchName = name,
            PropertyName = shape.PropertyName,
            CSharpType = shape.CSharpType,
            IsFlag = isFlag,
            AcceptsMultipleValues = shape.CSharpType.EndsWith("[]?", StringComparison.Ordinal),
            IsSecret = shape.IsSecret,
            ValueSeparator = !isFlag && name.StartsWith("--", StringComparison.Ordinal) ? "=" : " ",
            Phase = shape.Phase,
        };
    }

    private sealed record RootOptionShape(
        string PropertyName,
        string CSharpType = "bool?",
        bool IsSecret = false,
        CommandLinePhase Phase = CommandLinePhase.Normal);

    [GeneratedRegex(@"(?<![\w-])(--[a-z][a-z-]*|-[Cc])(?=[\s=\[\]|])")]
    private static partial Regex RootOptionPattern();
}
