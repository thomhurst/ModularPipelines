using System.Text.RegularExpressions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class GitCliScraper
{
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText)
    {
        // Only root usage declares inherited settings; command help reuses short switches.
        var synopsis = helpText.Split("These are common Git commands", StringSplitOptions.None)[0];
        var options = new List<CliOptionDefinition>();
        foreach (var match in RootOptionPattern().Matches(synopsis).Cast<Match>())
        {
            var name = match.Groups[1].Value;
            var property = name switch
            {
                "-C" => "ChangeDirectories",
                "-c" => "Configuration",
                "--config-env" => "ConfigEnv",
                "--exec-path" => "ExecPath",
                "--git-dir" => "GitDirectory",
                "--work-tree" => "WorkTree",
                "--namespace" => "Namespace",
                "--bare" => "BareRepository",
                "--paginate" => "Paginate",
                "--no-pager" => "NoPager",
                "--no-replace-objects" => "NoReplaceObjects",
                "--no-lazy-fetch" => "NoLazyFetch",
                "--no-optional-locks" => "NoOptionalLocks",
                "--no-advice" => "NoAdvice",
                _ => null,
            };
            if (property is null || options.Any(option => option.SwitchName == name))
            {
                continue;
            }

            var collection = name is "-C" or "-c" or "--config-env";
            var value = collection || name is "--exec-path" or "--git-dir" or "--work-tree" or "--namespace";
            options.Add(new CliOptionDefinition
            {
                SwitchName = name,
                PropertyName = property,
                CSharpType = name switch
                {
                    "-c" => "KeyValue[]?",
                    _ when collection => "string[]?",
                    _ when value => "string?",
                    _ => "bool?",
                },
                IsFlag = !value,
                AcceptsMultipleValues = collection,
                IsSecret = name == "-c",
                ValueSeparator = value && name.StartsWith("--", StringComparison.Ordinal) ? "=" : " ",
                Phase = name == "-C" ? CommandLinePhase.EarlyOperand : CommandLinePhase.Normal,
            });
        }

        return options;
    }

    [GeneratedRegex(@"(?<![\w-])(--[a-z][a-z-]*|-[Cc])(?=[\s=\[\]|])")]
    private static partial Regex RootOptionPattern();
}
