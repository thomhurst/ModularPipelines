using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator;

internal static class ToolCatalog
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

    public static IReadOnlyList<ToolCatalogEntry> Create(IEnumerable<ICliScraper> scrapers)
    {
        ArgumentNullException.ThrowIfNull(scrapers);

        var entries = scrapers
            .Select(CreateEntry)
            .OrderBy(entry => entry.ToolName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var duplicate = entries
            .GroupBy(entry => entry.ToolName, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);

        return duplicate is null
            ? entries
            : throw new InvalidOperationException(
                $"Multiple CLI scrapers are registered for tool '{duplicate.Key}'.");
    }

    public static string ToJson(IReadOnlyList<ToolCatalogEntry> entries) =>
        JsonSerializer.Serialize(entries, JsonOptions);

    public static string ToText(IReadOnlyList<ToolCatalogEntry> entries)
    {
        var lines = new[] { "Tool\tPackage\tNamespace prefix\tPlatform\tAutomation\tCommand facade" }
            .Concat(entries.Select(entry =>
                $"{entry.ToolName}\t{entry.PackageName}\t{entry.NamespacePrefix}\t"
                + $"{entry.GenerationPlatform}\t{entry.IncludeInGenerationMatrix}\t"
                + $"{entry.GenerateCommandFacade}"));
        return string.Join(Environment.NewLine, lines);
    }

    private static ToolCatalogEntry CreateEntry(ICliScraper scraper)
    {
        var toolName = RequireValue(scraper.ToolName, "tool name");
        var namespacePrefix = RequireValue(scraper.NamespacePrefix, $"namespace prefix for '{toolName}'");
        var outputDirectory = RequireValue(
                scraper.OutputDirectory,
                $"output directory for '{toolName}'")
            .Replace('\\', '/')
            .TrimEnd('/');
        var pathParts = outputDirectory.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (Path.IsPathRooted(outputDirectory)
            || pathParts.Length != 2
            || !string.Equals(pathParts[0], "src", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Tool '{toolName}' output directory must identify one project directly under src: "
                + $"'{outputDirectory}'.");
        }

        if (!Enum.IsDefined(scraper.GenerationPlatform))
        {
            throw new InvalidOperationException(
                $"Tool '{toolName}' has unsupported generation platform '{scraper.GenerationPlatform}'.");
        }

        return new ToolCatalogEntry(
            ToolName: toolName,
            PackageName: pathParts[1],
            NamespacePrefix: namespacePrefix,
            OutputDirectory: outputDirectory,
            GenerationPlatform: scraper.GenerationPlatform,
            IncludeInGenerationMatrix: scraper.IncludeInGenerationMatrix,
            GenerateCommandFacade: scraper.GenerateCommandFacade);
    }

    private static string RequireValue(string value, string description) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException($"CLI scraper {description} cannot be empty.")
            : value.Trim();
}

internal sealed record ToolCatalogEntry(
    string ToolName,
    string PackageName,
    string NamespacePrefix,
    string OutputDirectory,
    CliGenerationPlatform GenerationPlatform,
    bool IncludeInGenerationMatrix,
    bool GenerateCommandFacade);
