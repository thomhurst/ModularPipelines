using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates enum types for CLI options with constrained values. Members are emitted in a
/// deterministic order derived from their CLI values, so regenerating from the same allowed
/// value set never reorders members (or their ordinals) when a tool prints its values in an
/// unstable order.
/// </summary>
public class EnumGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var files = new List<GeneratedFile>();
        var generatedEnumNames = new HashSet<string>(StringComparer.Ordinal);

        foreach (var enumDef in tool.AllEnums)
        {
            AddEnumFile(files, generatedEnumNames, enumDef, tool);
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static void AddEnumFile(
        List<GeneratedFile> files,
        HashSet<string> generatedEnumNames,
        CliEnumDefinition enumDef,
        CliToolDefinition tool)
    {
        if (!generatedEnumNames.Add(enumDef.EnumName))
        {
            return;
        }

        files.Add(new GeneratedFile
        {
            RelativePath = Path.Combine(
                tool.OutputDirectory,
                "Enums",
                $"{enumDef.EnumName}.Generated.cs"),
            Content = GenerateEnum(enumDef, tool),
        });
    }

    private static string GenerateEnum(CliEnumDefinition enumDef, CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using ModularPipelines.Attributes;");
        sb.AppendLine();

        // Namespace
        sb.AppendLine($"namespace {tool.TargetNamespace}.Enums;");
        sb.AppendLine();

        // XML documentation
        if (!string.IsNullOrEmpty(enumDef.Description))
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"/// {GeneratorUtils.EscapeXmlComment(enumDef.Description)}");
            sb.AppendLine("/// </summary>");
        }

        sb.AppendLine(GeneratorUtils.GeneratedCodeAttribute);
        sb.AppendLine($"public enum {enumDef.EnumName}");
        sb.AppendLine("{");

        var values = GetUniqueValues(enumDef.Values);
        for (var i = 0; i < values.Count; i++)
        {
            var value = values[i];
            var isLast = i == values.Count - 1;

            if (!string.IsNullOrEmpty(value.Description))
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine($"    /// {GeneratorUtils.EscapeXmlComment(value.Description)}");
                sb.AppendLine("    /// </summary>");
            }

            // Add the attribute consumed by CommandArgumentBuilder at runtime.
            sb.AppendLine($"    [EnumValue({GeneratorUtils.FormatStringLiteral(value.CliValue)})]");
            sb.AppendLine($"    {value.MemberName}{(isLast ? "" : ",")}");

            if (!isLast)
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static IReadOnlyList<CliEnumValue> GetUniqueValues(IReadOnlyList<CliEnumValue> values)
    {
        var usedCliValues = new HashSet<string>(StringComparer.Ordinal);
        var usedMemberNames = new HashSet<string>(StringComparer.Ordinal);
        var uniqueValues = new List<CliEnumValue>();

        foreach (var value in CliEnumDefinition.OrderValues(values))
        {
            if (!usedCliValues.Add(value.CliValue))
            {
                continue;
            }

            var memberName = GetUniqueMemberName(value, usedMemberNames);
            uniqueValues.Add(value with { MemberName = memberName });
        }

        return uniqueValues;
    }

    private static string GetUniqueMemberName(CliEnumValue value, HashSet<string> usedMemberNames)
    {
        if (usedMemberNames.Add(value.MemberName))
        {
            return value.MemberName;
        }

        var casingSuffix = GetCasingSuffix(value.CliValue);
        var candidateRoot = value.MemberName + casingSuffix;
        var candidate = candidateRoot;
        for (var suffix = 2; !usedMemberNames.Add(candidate); suffix++)
        {
            candidate = candidateRoot + suffix;
        }

        return candidate;
    }

    private static string GetCasingSuffix(string cliValue)
    {
        var letters = cliValue.Where(char.IsLetter).ToArray();
        if (letters.Length > 0 && letters.All(char.IsUpper))
        {
            return "Uppercase";
        }

        if (letters.Length > 0 && letters.All(char.IsLower))
        {
            return "Lowercase";
        }

        return "Alternative";
    }
}
