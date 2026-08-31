using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

internal static class EnumReachabilityPruner
{
    public static CliToolDefinition PruneDiscardedEnumReferences(
        CliToolDefinition beforeCompatibility,
        CliToolDefinition afterCompatibility)
    {
        var referencedTypes = afterCompatibility.Commands
            .SelectMany(GetReferencedTypes)
            .Concat(afterCompatibility.GetGlobalOptions().Select(option => option.CSharpType))
            .Concat(afterCompatibility.GlobalCompatibilityProperties.Select(property => property.CSharpType))
            .ToArray();
        var discardedEnumNames = beforeCompatibility.Commands
            .SelectMany(command => command.Options)
            .Concat(beforeCompatibility.GetGlobalOptions())
            .Where(option => option.EnumDefinition is not null)
            .Select(option => option.EnumDefinition!.EnumName)
            .Where(enumName => !referencedTypes.Any(type => ReferencesType(type, enumName)))
            .ToHashSet(StringComparer.Ordinal);

        return afterCompatibility with
        {
            Commands = [.. afterCompatibility.Commands.Select(command => command with
            {
                Enums = [.. command.Enums.Where(definition =>
                    !discardedEnumNames.Contains(definition.EnumName))],
                Options = [.. command.Options.Select(option =>
                    option.EnumDefinition is not null
                    && discardedEnumNames.Contains(option.EnumDefinition.EnumName)
                        ? option with { EnumDefinition = null }
                        : option)],
            })],
            GlobalOptions = [.. afterCompatibility.GlobalOptions.Select(option =>
                option.EnumDefinition is not null
                && discardedEnumNames.Contains(option.EnumDefinition.EnumName)
                    ? option with { EnumDefinition = null }
                    : option)],
        };
    }

    private static IEnumerable<string> GetReferencedTypes(CliCommandDefinition command) =>
        command.Options.Select(option => option.CSharpType)
            .Concat(command.PositionalArguments.Select(argument => argument.CSharpType))
            .Concat(command.CompatibilityProperties.Select(property => property.CSharpType))
            .Concat(command.CompatibilityConstructors.SelectMany(constructor =>
                constructor.Parameters.Select(parameter => parameter.CSharpType)))
            .Concat(command.AliasCompatibilityProperties.Values.SelectMany(properties =>
                properties.SelectMany(property => new[]
                {
                    property.AliasCSharpType,
                    property.CanonicalCSharpType,
                })))
            .Concat(command.AliasCompatibilityConstructors.Values.SelectMany(constructors =>
                constructors.SelectMany(constructor =>
                    constructor.Parameters.Select(parameter => parameter.CSharpType))));

    private static bool ReferencesType(string csharpType, string typeName) =>
        csharpType.Split(['<', '>', ',', '?', '[', ']', ' '], StringSplitOptions.RemoveEmptyEntries)
            .Any(candidate => candidate.Equals(typeName, StringComparison.Ordinal)
                              || candidate.EndsWith($".{typeName}", StringComparison.Ordinal));
}
