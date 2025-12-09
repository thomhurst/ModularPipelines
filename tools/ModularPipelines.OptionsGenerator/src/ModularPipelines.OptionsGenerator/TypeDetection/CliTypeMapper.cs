using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Centralized utility for mapping between CLI type representations.
/// Consolidates type mapping logic to ensure consistency across detectors.
/// </summary>
public static class CliTypeMapper
{
    /// <summary>
    /// Maps Cobra CLI type hints to CliOptionType.
    /// Cobra format shows type after flag: --name string, --verbose (no type = bool)
    /// </summary>
    public static CliOptionType FromCobraTypeHint(string? cobraType)
    {
        if (string.IsNullOrWhiteSpace(cobraType))
        {
            return CliOptionType.Bool; // No type hint = boolean flag
        }

        var normalized = cobraType.Trim().ToLowerInvariant();

        return normalized switch
        {
            // Explicit boolean
            "bool" or "boolean" => CliOptionType.Bool,

            // Integer types
            "int" or "int32" or "int64" or "uint" or "uint32" or "uint64" => CliOptionType.Int,

            // Float types
            "float" or "float32" or "float64" => CliOptionType.Decimal,

            // Array/list types
            "list" or "array" or "strings" or "stringarray" or "stringslice" => CliOptionType.StringList,

            // Map types
            "map" or "stringtostring" => CliOptionType.KeyValue,

            // Duration and other string-like types
            "duration" or "string" or "bytes" => CliOptionType.String,

            // Default to string for unknown types
            _ => CliOptionType.String
        };
    }

    /// <summary>
    /// Parses type names from various sources (JSON, descriptions) to CliOptionType.
    /// </summary>
    public static CliOptionType FromTypeName(string? typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            return CliOptionType.Unknown;
        }

        var normalized = typeName.Trim().ToLowerInvariant();

        return normalized switch
        {
            "bool" or "boolean" => CliOptionType.Bool,
            "string" => CliOptionType.String,
            "int" or "integer" => CliOptionType.Int,
            "decimal" or "float" or "double" => CliOptionType.Decimal,
            "stringlist" or "list" or "array" => CliOptionType.StringList,
            "keyvalue" or "map" or "dictionary" => CliOptionType.KeyValue,
            "enum" or "enumeration" => CliOptionType.Enum,
            _ => CliOptionType.Unknown
        };
    }

    /// <summary>
    /// Maps CliOptionType to C# type string for code generation.
    /// </summary>
    public static string ToCSharpType(CliOptionType type, CliEnumDefinition? enumDef = null)
    {
        return type switch
        {
            CliOptionType.Bool => "bool?",
            CliOptionType.Int => "int?",
            CliOptionType.Decimal => "decimal?",
            CliOptionType.StringList => "string[]?",
            CliOptionType.KeyValue => "IEnumerable<KeyValue>?",
            CliOptionType.Enum when enumDef is not null => $"{enumDef.EnumName}?",
            CliOptionType.Enum => "string?", // Fallback when no enum definition provided
            _ when enumDef is not null => $"{enumDef.EnumName}?", // Any type with enum def
            _ => "string?"
        };
    }

    /// <summary>
    /// Gets the canonical string name for a CliOptionType (for serialization).
    /// </summary>
    public static string ToTypeName(CliOptionType type)
    {
        return type switch
        {
            CliOptionType.Bool => "bool",
            CliOptionType.String => "string",
            CliOptionType.Int => "int",
            CliOptionType.Decimal => "decimal",
            CliOptionType.StringList => "stringlist",
            CliOptionType.KeyValue => "keyvalue",
            CliOptionType.Enum => "enum",
            _ => "unknown"
        };
    }

    /// <summary>
    /// Known Cobra type hints that are valid.
    /// </summary>
    public static readonly HashSet<string> KnownCobraTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "string", "int", "int32", "int64", "uint", "uint32", "uint64",
        "float", "float32", "float64", "bool", "boolean",
        "list", "array", "strings", "stringarray", "stringslice",
        "map", "stringtostring",
        "duration", "bytes"
    };

    /// <summary>
    /// Checks if a type hint is a known Cobra type.
    /// </summary>
    public static bool IsKnownCobraType(string? typeHint)
    {
        if (string.IsNullOrWhiteSpace(typeHint))
        {
            return true; // Empty is valid (boolean)
        }

        return KnownCobraTypes.Contains(typeHint.Trim());
    }
}
