using ModularPipelines.Enums;

namespace ModularPipelines.PipelineCli;

internal static class PipelineCommandLineParser
{
    private const string ListModulesOption = "--list-modules";
    private const string ValidateOption = "--validate";
    private const string DryRunOption = "--dry-run";
    private const string ModuleOption = "--module";
    private const string SkipModuleOption = "--skip-module";
    private const string CategoriesOption = "--categories";
    private const string IgnoreCategoriesOption = "--ignore-categories";
    private const string GraphOption = "--graph";
    private const string GraphPathOption = "--graph-path";

    public static PipelineCommandLineOptions Parse(IReadOnlyList<string>? arguments)
    {
        if (arguments is null || arguments.Count == 0)
        {
            return PipelineCommandLineOptions.Empty;
        }

        var command = PipelineCommand.Run;
        var hostArguments = new List<string>();
        var targetModules = new List<string>();
        var skippedModules = new List<string>();
        var runOnlyCategories = new List<string>();
        var ignoreCategories = new List<string>();
        DependencyGraphFormat? graphFormat = null;
        string? graphPath = null;

        for (var index = 0; index < arguments.Count; index++)
        {
            var argument = arguments[index];
            if (argument.Equals(ListModulesOption, StringComparison.OrdinalIgnoreCase))
            {
                command = SetCommand(command, PipelineCommand.ListModules, argument);
                continue;
            }

            if (argument.Equals(ValidateOption, StringComparison.OrdinalIgnoreCase))
            {
                command = SetCommand(command, PipelineCommand.Validate, argument);
                continue;
            }

            if (TryReadGraph(arguments, ref index, out var parsedGraphFormat, out var parsedGraphPath))
            {
                command = SetCommand(command, PipelineCommand.ExportGraph, argument);
                graphFormat = parsedGraphFormat;
                if (parsedGraphPath is not null)
                {
                    graphPath = SetGraphPath(graphPath, parsedGraphPath);
                }

                continue;
            }

            if (TryReadGraphPath(arguments, ref index, out var parsedExplicitGraphPath))
            {
                graphPath = SetGraphPath(graphPath, parsedExplicitGraphPath);
                continue;
            }

            if (argument.Equals(DryRunOption, StringComparison.OrdinalIgnoreCase))
            {
                command = SetCommand(command, PipelineCommand.DryRun, argument);
                continue;
            }

            if (TryReadValues(arguments, ref index, ModuleOption, targetModules)
                || TryReadValues(arguments, ref index, SkipModuleOption, skippedModules)
                || TryReadValues(arguments, ref index, CategoriesOption, runOnlyCategories)
                || TryReadValues(arguments, ref index, IgnoreCategoriesOption, ignoreCategories))
            {
                continue;
            }

            hostArguments.Add(argument);
        }

        if (graphPath is not null && graphFormat is null)
        {
            throw new ArgumentException(
                $"Command-line option '{GraphPathOption}' requires '{GraphOption}'.",
                nameof(arguments));
        }

        if (graphFormat is { } resolvedGraphFormat)
        {
            graphPath ??= GetDefaultGraphPath(resolvedGraphFormat);
        }

        return new PipelineCommandLineOptions(
            command,
            hostArguments,
            Distinct(targetModules),
            Distinct(skippedModules),
            Distinct(runOnlyCategories),
            Distinct(ignoreCategories),
            graphFormat,
            graphPath);
    }

    private static bool TryReadGraph(
        IReadOnlyList<string> arguments,
        ref int index,
        out DependencyGraphFormat format,
        out string? path)
    {
        var argument = arguments[index];
        string? value;
        if (argument.Equals(GraphOption, StringComparison.OrdinalIgnoreCase))
        {
            if (++index >= arguments.Count || arguments[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"Command-line option '{GraphOption}' requires mermaid, dot, or json.",
                    nameof(arguments));
            }

            value = arguments[index];
        }
        else if (argument.StartsWith($"{GraphOption}=", StringComparison.OrdinalIgnoreCase))
        {
            value = argument[(GraphOption.Length + 1)..];
        }
        else
        {
            format = default;
            path = null;
            return false;
        }

        if (value.Equals("mermaid", StringComparison.OrdinalIgnoreCase))
        {
            format = DependencyGraphFormat.Mermaid;
        }
        else if (value.Equals("dot", StringComparison.OrdinalIgnoreCase))
        {
            format = DependencyGraphFormat.Dot;
        }
        else if (value.Equals("json", StringComparison.OrdinalIgnoreCase))
        {
            format = DependencyGraphFormat.Json;
        }
        else
        {
            throw new ArgumentException(
                $"Unsupported dependency graph format '{value}'. Use mermaid, dot, or json.",
                nameof(arguments));
        }

        path = index + 1 < arguments.Count
               && IsGraphPath(arguments[index + 1])
            ? arguments[++index]
            : null;
        return true;
    }

    private static bool IsGraphPath(string argument) =>
        !argument.StartsWith("--", StringComparison.Ordinal)
        && (!argument.Contains('=')
            || argument.Contains(Path.DirectorySeparatorChar)
            || argument.Contains(Path.AltDirectorySeparatorChar));

    private static bool TryReadGraphPath(
        IReadOnlyList<string> arguments,
        ref int index,
        out string path)
    {
        var argument = arguments[index];
        if (argument.Equals(GraphPathOption, StringComparison.OrdinalIgnoreCase))
        {
            if (++index >= arguments.Count || arguments[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"Command-line option '{GraphPathOption}' requires a path.",
                    nameof(arguments));
            }

            path = arguments[index];
            return true;
        }

        if (argument.StartsWith($"{GraphPathOption}=", StringComparison.OrdinalIgnoreCase))
        {
            path = argument[(GraphPathOption.Length + 1)..];
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(
                    $"Command-line option '{GraphPathOption}' requires a path.",
                    nameof(arguments));
            }

            return true;
        }

        path = string.Empty;
        return false;
    }

    private static string SetGraphPath(string? current, string requested)
    {
        if (current is not null)
        {
            throw new ArgumentException("A dependency graph path can only be specified once.");
        }

        return requested;
    }

    private static string GetDefaultGraphPath(DependencyGraphFormat format) =>
        format switch
        {
            DependencyGraphFormat.Mermaid => "dependency-graph.mmd",
            DependencyGraphFormat.Dot => "dependency-graph.dot",
            DependencyGraphFormat.Json => "dependency-graph.json",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null),
        };

    private static bool TryReadValues(
        IReadOnlyList<string> arguments,
        ref int index,
        string option,
        List<string> destination)
    {
        var argument = arguments[index];
        string? value = null;
        if (argument.Equals(option, StringComparison.OrdinalIgnoreCase))
        {
            if (++index >= arguments.Count || arguments[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Command-line option '{option}' requires a value.", nameof(arguments));
            }

            value = arguments[index];
        }
        else if (argument.StartsWith($"{option}=", StringComparison.OrdinalIgnoreCase))
        {
            value = argument[(option.Length + 1)..];
        }
        else
        {
            return false;
        }

        var values = IsAssemblyQualifiedModuleName(option, value)
            ? [value.Trim()]
            : value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (values.Length == 0)
        {
            throw new ArgumentException($"Command-line option '{option}' requires a non-empty value.", nameof(arguments));
        }

        destination.AddRange(values);
        return true;
    }

    private static bool IsAssemblyQualifiedModuleName(string option, string value)
    {
        if (!option.Equals(ModuleOption, StringComparison.OrdinalIgnoreCase)
            && !option.Equals(SkipModuleOption, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return value.Split(',').Skip(1).Any(static part =>
        {
            var component = part.TrimStart();
            return component.StartsWith("Version=", StringComparison.OrdinalIgnoreCase)
                   || component.StartsWith("Culture=", StringComparison.OrdinalIgnoreCase)
                   || component.StartsWith("PublicKeyToken=", StringComparison.OrdinalIgnoreCase);
        });
    }

    private static PipelineCommand SetCommand(
        PipelineCommand current,
        PipelineCommand requested,
        string option)
    {
        if (current != PipelineCommand.Run && current != requested)
        {
            throw new ArgumentException(
                $"Command-line option '{option}' cannot be combined with another pipeline command.");
        }

        return requested;
    }

    private static IReadOnlyList<string> Distinct(IEnumerable<string> values) =>
        values.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
}
