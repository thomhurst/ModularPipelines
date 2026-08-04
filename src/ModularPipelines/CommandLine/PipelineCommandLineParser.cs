namespace ModularPipelines.PipelineCli;

internal static class PipelineCommandLineParser
{
    private const string HelpOption = "--help";
    private const string ShortHelpOption = "-h";
    private const string ListModulesOption = "--list-modules";
    private const string ValidateOption = "--validate";
    private const string DryRunOption = "--dry-run";
    private const string ModuleOption = "--module";
    private const string SkipModuleOption = "--skip-module";
    private const string CategoriesOption = "--categories";
    private const string IgnoreCategoriesOption = "--ignore-categories";

    private static readonly string[] KnownLongOptions =
    [
        HelpOption,
        ListModulesOption,
        ValidateOption,
        DryRunOption,
        ModuleOption,
        SkipModuleOption,
        CategoriesOption,
        IgnoreCategoriesOption,
    ];

    private static readonly string[] FlagOptions =
    [
        HelpOption,
        ListModulesOption,
        ValidateOption,
        DryRunOption,
    ];

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

        for (var index = 0; index < arguments.Count; index++)
        {
            var argument = arguments[index];
            if (argument == "--")
            {
                hostArguments.AddRange(arguments.Skip(index + 1));
                break;
            }

            if (argument.Equals(HelpOption, StringComparison.OrdinalIgnoreCase)
                || argument.Equals(ShortHelpOption, StringComparison.OrdinalIgnoreCase))
            {
                command = SetCommand(command, PipelineCommand.Help, argument);
                continue;
            }

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

            ThrowForLikelyPipelineOptionTypo(argument);

            hostArguments.Add(argument);
        }

        return new PipelineCommandLineOptions(
            command,
            hostArguments,
            Distinct(targetModules),
            Distinct(skippedModules),
            Distinct(runOnlyCategories),
            Distinct(ignoreCategories));
    }

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
                throw CreateParseException($"Command-line option '{option}' requires a value.");
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
            throw CreateParseException($"Command-line option '{option}' requires a non-empty value.");
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
            throw CreateParseException(
                $"Command-line option '{option}' cannot be combined with another pipeline command.");
        }

        return requested;
    }

    private static IReadOnlyList<string> Distinct(IEnumerable<string> values) =>
        values.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

    private static void ThrowForLikelyPipelineOptionTypo(string argument)
    {
        if (!argument.StartsWith("--", StringComparison.Ordinal))
        {
            return;
        }

        var equalsIndex = argument.IndexOf('=', StringComparison.Ordinal);
        var option = equalsIndex < 0 ? argument : argument[..equalsIndex];
        var flagOption = FlagOptions.FirstOrDefault(
            knownOption => option.Equals(knownOption, StringComparison.OrdinalIgnoreCase));
        if (equalsIndex >= 0 && flagOption is not null)
        {
            throw CreateParseException(
                $"Command-line option '{flagOption}' does not accept a value.");
        }

        var maximumDistance = option.Length switch
        {
            <= 7 => 1,
            <= 12 => 2,
            _ => 3,
        };
        var suggestion = KnownLongOptions
            .Select(knownOption => new
            {
                Option = knownOption,
                Distance = GetEditDistance(option, knownOption),
            })
            .Where(candidate => candidate.Distance <= maximumDistance)
            .OrderBy(candidate => candidate.Distance)
            .ThenBy(candidate => candidate.Option, StringComparer.Ordinal)
            .FirstOrDefault();

        if (suggestion is not null)
        {
            throw CreateParseException(
                $"Unknown pipeline option '{option}'. Did you mean '{suggestion.Option}'? "
                + "Use '--' before the option to forward it to host configuration.");
        }
    }

    private static int GetEditDistance(string left, string right)
    {
        var previous = Enumerable.Range(0, right.Length + 1).ToArray();

        for (var leftIndex = 1; leftIndex <= left.Length; leftIndex++)
        {
            var current = new int[right.Length + 1];
            current[0] = leftIndex;

            for (var rightIndex = 1; rightIndex <= right.Length; rightIndex++)
            {
                var substitutionCost = char.ToUpperInvariant(left[leftIndex - 1])
                    == char.ToUpperInvariant(right[rightIndex - 1])
                    ? 0
                    : 1;
                current[rightIndex] = Math.Min(
                    Math.Min(current[rightIndex - 1] + 1, previous[rightIndex] + 1),
                    previous[rightIndex - 1] + substitutionCost);
            }

            previous = current;
        }

        return previous[right.Length];
    }

    private static ArgumentException CreateParseException(string message) =>
        new($"{message}{Environment.NewLine}{Environment.NewLine}{PipelineCommandLineHelp.Usage}");
}
