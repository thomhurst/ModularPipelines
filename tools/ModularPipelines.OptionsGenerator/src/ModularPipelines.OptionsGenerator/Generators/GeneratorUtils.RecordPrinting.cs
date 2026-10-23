using System.Text;

namespace ModularPipelines.OptionsGenerator.Generators;

public static partial class GeneratorUtils
{
    internal static void GenerateRedactingPrintMembers(StringBuilder builder)
    {
        // Do not evaluate option getters or base printers: even unannotated values can
        // contain credentials in URLs, keyed values, or custom collection ToString methods.
        builder.AppendLine("""
                /// <summary>Appends diagnostic members without exposing command option values.</summary>
                /// <param name="builder">The builder receiving the record members.</param>
                /// <returns>True because a redaction marker was appended.</returns>
                protected override bool PrintMembers(global::System.Text.StringBuilder builder)
                {
                    builder.Append("Options = <redacted>");
                    return true;
                }

            """);
    }
}
