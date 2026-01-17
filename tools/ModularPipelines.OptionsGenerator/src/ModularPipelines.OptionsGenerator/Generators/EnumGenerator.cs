using System.Text;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Generates enum types for CLI options with constrained values.
/// </summary>
public class EnumGenerator : ICodeGenerator
{
    public Task<IReadOnlyList<GeneratedFile>> GenerateAsync(CliToolDefinition tool, CancellationToken cancellationToken = default)
    {
        var files = new List<GeneratedFile>();

        foreach (var enumDef in tool.AllEnums)
        {
            var content = GenerateEnum(enumDef, tool);
            var fileName = $"{enumDef.EnumName}.Generated.cs";
            var relativePath = Path.Combine(tool.OutputDirectory, "Enums", fileName);

            files.Add(new GeneratedFile
            {
                RelativePath = relativePath,
                Content = content
            });
        }

        return Task.FromResult<IReadOnlyList<GeneratedFile>>(files);
    }

    private static string GenerateEnum(CliEnumDefinition enumDef, CliToolDefinition tool)
    {
        var sb = new StringBuilder();

        // File header
        GeneratorUtils.GenerateFileHeader(sb);

        sb.AppendLine("using System.CodeDom.Compiler;");
        sb.AppendLine("using System.ComponentModel;");
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

        for (var i = 0; i < enumDef.Values.Count; i++)
        {
            var value = enumDef.Values[i];
            var isLast = i == enumDef.Values.Count - 1;

            if (!string.IsNullOrEmpty(value.Description))
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine($"    /// {GeneratorUtils.EscapeXmlComment(value.Description)}");
                sb.AppendLine("    /// </summary>");
            }

            // Add Description attribute with the CLI value for serialization
            // Escape special characters in the CLI value for use in string literal
            var escapedCliValue = EscapeStringLiteral(value.CliValue);
            sb.AppendLine($"    [Description(\"{escapedCliValue}\")]");
            sb.AppendLine($"    {value.MemberName}{(isLast ? "" : ",")}");

            if (!isLast)
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");

        return sb.ToString();
    }

    /// <summary>
    /// Escapes a string for use in a C# string literal.
    /// Handles newlines, carriage returns, tabs, backslashes, quotes, and other control characters.
    /// </summary>
    private static string EscapeStringLiteral(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var sb = new StringBuilder(value.Length + 10);

        foreach (var c in value)
        {
            switch (c)
            {
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '"':
                    sb.Append("\\\"");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\0':
                    sb.Append("\\0");
                    break;
                case '\a':
                    sb.Append("\\a");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                case '\v':
                    sb.Append("\\v");
                    break;
                default:
                    // Escape other control characters and Unicode line/paragraph separators
                    if (char.IsControl(c) || c == '\u2028' || c == '\u2029')
                    {
                        sb.Append($"\\u{(int)c:X4}");
                    }
                    else
                    {
                        sb.Append(c);
                    }

                    break;
            }
        }

        return sb.ToString();
    }
}
