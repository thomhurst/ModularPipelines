using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Base;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Orchestrates the scraping and code generation process.
/// Supports both CLI-first scrapers (preferred) and HTML scrapers (fallback).
/// </summary>
public class CodeGeneratorOrchestrator
{
    private readonly IEnumerable<ICliScraper> _cliScrapers;
    private readonly IEnumerable<ICliDocumentationScraper> _htmlScrapers;
    private readonly IEnumerable<ICodeGenerator> _generators;
    private readonly ILogger<CodeGeneratorOrchestrator> _logger;
    private readonly OptionTypeEnhancer? _typeEnhancer;

    public CodeGeneratorOrchestrator(
        IEnumerable<ICliScraper> cliScrapers,
        IEnumerable<ICliDocumentationScraper> htmlScrapers,
        IEnumerable<ICodeGenerator> generators,
        ILogger<CodeGeneratorOrchestrator> logger,
        OptionTypeEnhancer? typeEnhancer = null)
    {
        ArgumentNullException.ThrowIfNull(cliScrapers);
        ArgumentNullException.ThrowIfNull(htmlScrapers);
        ArgumentNullException.ThrowIfNull(generators);
        ArgumentNullException.ThrowIfNull(logger);

        _cliScrapers = cliScrapers;
        _htmlScrapers = htmlScrapers;
        _generators = generators;
        _logger = logger;
        _typeEnhancer = typeEnhancer;
    }

    /// <summary>
    /// Generates code for the specified tools.
    /// </summary>
    /// <param name="toolsToGenerate">Comma-separated list of tools or "all".</param>
    /// <param name="outputDirectory">Output directory path.</param>
    /// <param name="useCliFirst">When true, prefer CLI scrapers over HTML scrapers.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<GenerationResult> GenerateAsync(
        string toolsToGenerate,
        string outputDirectory,
        bool useCliFirst = true,
        CancellationToken cancellationToken = default)
    {
        var result = new GenerationResult();
        var toolList = ParseToolList(toolsToGenerate);
        var processedTools = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Build a lookup of CLI scrapers by tool name
        var cliScrapersByTool = _cliScrapers.ToDictionary(s => s.ToolName, s => s, StringComparer.OrdinalIgnoreCase);

        // Process each HTML scraper (as the baseline list of tools)
        foreach (var htmlScraper in _htmlScrapers)
        {
            if (!ShouldProcess(htmlScraper.ToolName, toolList))
            {
                _logger.LogInformation("Skipping {Tool}", htmlScraper.ToolName);
                continue;
            }

            if (processedTools.Contains(htmlScraper.ToolName))
            {
                continue;
            }

            _logger.LogInformation("Processing {Tool}...", htmlScraper.ToolName);

            try
            {
                processedTools.Add(htmlScraper.ToolName);
                result.ToolsProcessed.Add(htmlScraper.ToolName);

                // Try CLI scraper first if enabled (streaming)
                if (useCliFirst && cliScrapersByTool.TryGetValue(htmlScraper.ToolName, out var cliScraper))
                {
                    if (await cliScraper.IsAvailableAsync(cancellationToken))
                    {
                        _logger.LogInformation("Using CLI scraper for {Tool}", htmlScraper.ToolName);

                        var toolDefinition = cliScraper.CreateToolDefinition();

                        // Clean up old generated files before generating new ones
                        CleanupGeneratedFiles(outputDirectory, toolDefinition.OutputDirectory, toolDefinition.NamespacePrefix);
                        var allCommands = new List<CliCommandDefinition>();

                        // Collect all commands first
                        await foreach (var command in cliScraper.ScrapeAsync(cancellationToken))
                        {
                            allCommands.Add(command);
                        }

                        _logger.LogInformation("Scraped {Count} commands for {Tool}", allCommands.Count, htmlScraper.ToolName);

                        // If no commands found, fall through to HTML scraper
                        if (allCommands.Count == 0)
                        {
                            _logger.LogWarning("CLI scraper for {Tool} returned no commands, falling back to HTML scraper", htmlScraper.ToolName);
                        }
                        else
                        {
                        // Create complete tool definition
                        var completeToolDefinition = new CliToolDefinition
                        {
                            ToolName = toolDefinition.ToolName,
                            NamespacePrefix = toolDefinition.NamespacePrefix,
                            TargetNamespace = toolDefinition.TargetNamespace,
                            OutputDirectory = toolDefinition.OutputDirectory,
                            Commands = allCommands,
                            Errors = []
                        };

                        // Run all generators with complete tool definition
                        foreach (var generator in _generators)
                        {
                            var files = await generator.GenerateAsync(completeToolDefinition, cancellationToken);

                            foreach (var file in files)
                            {
                                var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                                await WriteFileAsync(fullPath, file.Content, cancellationToken);
                                result.FilesGenerated.Add(file.RelativePath);
                            }
                        }

                        // Generate AssemblyInfo with generation metadata
                        await WriteAssemblyInfoAsync(outputDirectory, completeToolDefinition, cancellationToken);
                        result.FilesGenerated.Add(Path.Combine(completeToolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs"));

                        _logger.LogInformation("Generated files for {Tool} ({Count} commands, {SubDomainCount} sub-domains)",
                            htmlScraper.ToolName, allCommands.Count, completeToolDefinition.SubDomainGroups.Count);
                        continue;
                        }
                    }

                    _logger.LogWarning("{Tool} CLI not available, falling back to HTML scraper", htmlScraper.ToolName);
                }

                // Fall back to HTML scraper (batch mode)
                _logger.LogInformation("Using HTML scraper for {Tool}", htmlScraper.ToolName);

                // Clean up old generated files before generating new ones
                CleanupGeneratedFiles(outputDirectory, htmlScraper.OutputDirectory, htmlScraper.NamespacePrefix);

                var htmlToolDefinition = await htmlScraper.ScrapeAsync(cancellationToken);

                if (htmlToolDefinition.Errors.Count > 0)
                {
                    result.Errors.AddRange(htmlToolDefinition.Errors);
                    _logger.LogWarning("Encountered {Count} errors while scraping {Tool}",
                        htmlToolDefinition.Errors.Count, htmlScraper.ToolName);
                }

                _logger.LogInformation("Scraped {Count} commands for {Tool}",
                    htmlToolDefinition.Commands.Count, htmlScraper.ToolName);

                // Enhance types if enhancer is available
                if (_typeEnhancer is not null)
                {
                    _logger.LogInformation("Enhancing types for {Tool} using CLI help...", htmlScraper.ToolName);
                    try
                    {
                        htmlToolDefinition = await _typeEnhancer.EnhanceAsync(htmlToolDefinition, cancellationToken);
                        _logger.LogInformation("Type enhancement complete for {Tool}", htmlScraper.ToolName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Type enhancement failed for {Tool}, using scraped types", htmlScraper.ToolName);
                    }
                }

                // Generate code using all generators
                foreach (var generator in _generators)
                {
                    var files = await generator.GenerateAsync(htmlToolDefinition, cancellationToken);

                    foreach (var file in files)
                    {
                        var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                        await WriteFileAsync(fullPath, file.Content, cancellationToken);
                        result.FilesGenerated.Add(file.RelativePath);
                    }
                }

                // Generate AssemblyInfo with generation metadata
                await WriteAssemblyInfoAsync(outputDirectory, htmlToolDefinition, cancellationToken);
                result.FilesGenerated.Add(Path.Combine(htmlToolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs"));

                _logger.LogInformation("Generated files for {Tool}", htmlScraper.ToolName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process {Tool}", htmlScraper.ToolName);
                result.Errors.Add(new ScrapingError
                {
                    Source = htmlScraper.ToolName,
                    Message = ex.Message,
                    Exception = ex
                });
            }
        }

        // Process CLI-only scrapers (tools that have no HTML scraper)
        if (useCliFirst)
        {
            foreach (var cliScraper in _cliScrapers)
            {
                if (processedTools.Contains(cliScraper.ToolName))
                {
                    continue;
                }

                if (!ShouldProcess(cliScraper.ToolName, toolList))
                {
                    _logger.LogInformation("Skipping CLI-only tool {Tool}", cliScraper.ToolName);
                    continue;
                }

                _logger.LogInformation("Processing CLI-only tool {Tool}...", cliScraper.ToolName);

                try
                {
                    if (!await cliScraper.IsAvailableAsync(cancellationToken))
                    {
                        _logger.LogWarning("{Tool} CLI not available, skipping", cliScraper.ToolName);
                        continue;
                    }

                    var toolDefinition = cliScraper.CreateToolDefinition();

                    // Clean up old generated files before generating new ones
                    CleanupGeneratedFiles(outputDirectory, toolDefinition.OutputDirectory, toolDefinition.NamespacePrefix);

                    var allCommands = new List<CliCommandDefinition>();

                    processedTools.Add(cliScraper.ToolName);
                    result.ToolsProcessed.Add(cliScraper.ToolName);

                    // Collect all commands first
                    await foreach (var command in cliScraper.ScrapeAsync(cancellationToken))
                    {
                        allCommands.Add(command);
                    }

                    _logger.LogInformation("Scraped {Count} commands for {Tool}", allCommands.Count, cliScraper.ToolName);

                    // Throw error if no commands were found - CLI-only tools have no fallback
                    if (allCommands.Count == 0)
                    {
                        var errorMessage = $"No commands found for {cliScraper.ToolName}. This is a CLI-only tool with no HTML fallback. Ensure the CLI tool is installed and accessible.";
                        _logger.LogError(errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }

                    // Create complete tool definition
                    var completeToolDefinition = new CliToolDefinition
                    {
                        ToolName = toolDefinition.ToolName,
                        NamespacePrefix = toolDefinition.NamespacePrefix,
                        TargetNamespace = toolDefinition.TargetNamespace,
                        OutputDirectory = toolDefinition.OutputDirectory,
                        Commands = allCommands,
                        Errors = []
                    };

                    // Run all generators with complete tool definition
                    foreach (var generator in _generators)
                    {
                        var files = await generator.GenerateAsync(completeToolDefinition, cancellationToken);

                        foreach (var file in files)
                        {
                            var fullPath = Path.Combine(outputDirectory, file.RelativePath);
                            await WriteFileAsync(fullPath, file.Content, cancellationToken);
                            result.FilesGenerated.Add(file.RelativePath);
                        }
                    }

                    // Generate AssemblyInfo with generation metadata
                    await WriteAssemblyInfoAsync(outputDirectory, completeToolDefinition, cancellationToken);
                    result.FilesGenerated.Add(Path.Combine(completeToolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs"));

                    _logger.LogInformation("Generated files for {Tool} ({Count} commands, {SubDomainCount} sub-domains)",
                        cliScraper.ToolName, allCommands.Count, completeToolDefinition.SubDomainGroups.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process CLI-only tool {Tool}", cliScraper.ToolName);
                    result.Errors.Add(new ScrapingError
                    {
                        Source = cliScraper.ToolName,
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }
        }

        return result;
    }

    private static List<string> ParseToolList(string tools)
    {
        if (string.Equals(tools, "all", StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        return tools.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToLowerInvariant())
            .ToList();
    }

    private static bool ShouldProcess(string toolName, List<string> toolList)
    {
        if (toolList.Count == 0)
        {
            return true; // Process all
        }

        return toolList.Contains(toolName.ToLowerInvariant());
    }

    private static async Task WriteFileAsync(string path, string content, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(path, content, cancellationToken);
    }

    /// <summary>
    /// Generates and writes AssemblyInfo.Generated.cs with generation metadata.
    /// This centralizes the generation timestamp in one file per assembly rather than in every generated file,
    /// reducing unnecessary diffs when regenerating unchanged files.
    /// </summary>
    private static async Task WriteAssemblyInfoAsync(
        string outputDirectory,
        CliToolDefinition toolDefinition,
        CancellationToken cancellationToken)
    {
        var content = GeneratorUtils.GenerateAssemblyInfo(toolDefinition.TargetNamespace, toolDefinition.ToolName);
        var path = Path.Combine(outputDirectory, toolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs");
        await WriteFileAsync(path, content, cancellationToken);
    }

    /// <summary>
    /// Cleans up old generated files before regenerating.
    /// Only deletes files that match the namespace prefix to avoid removing files from other tools
    /// that share the same output directory (e.g., kubectl and kustomize in ModularPipelines.Kubernetes).
    /// Hand-written files (without auto-generated header) are preserved unless they match the prefix.
    /// </summary>
    private void CleanupGeneratedFiles(string outputDirectory, string toolOutputDirectory, string namespacePrefix)
    {
        var toolPath = Path.Combine(outputDirectory, toolOutputDirectory);
        if (!Directory.Exists(toolPath))
        {
            return;
        }

        var generatedDirs = new[] { "Options", "Services", "Extensions", "Enums" };

        foreach (var subDir in generatedDirs)
        {
            var subDirPath = Path.Combine(toolPath, subDir);
            if (!Directory.Exists(subDirPath))
            {
                continue;
            }

            // Delete *.Generated.cs files that match the namespace prefix
            // This ensures we don't delete files from other tools sharing the same directory
            // (e.g., running kustomize won't delete KubernetesOptions.Generated.cs which kubectl needs)
            var generatedFiles = Directory.GetFiles(subDirPath, "*.Generated.cs", SearchOption.TopDirectoryOnly);
            foreach (var file in generatedFiles)
            {
                var fileName = Path.GetFileName(file);
                if (FileMatchesNamespacePrefix(fileName, namespacePrefix))
                {
                    DeleteFileIfExists(file);
                }
            }

            // Also clean up legacy files that will be replaced by generated files
            // This handles transition from hand-written to generated files
            var allCsFiles = Directory.GetFiles(subDirPath, "*.cs", SearchOption.TopDirectoryOnly);
            foreach (var file in allCsFiles)
            {
                if (file.EndsWith(".Generated.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Already handled above
                }

                var fileName = Path.GetFileName(file);

                // Delete if it's a legacy auto-generated file that matches the prefix
                if (FileMatchesNamespacePrefix(fileName, namespacePrefix) && IsAutoGeneratedFile(file))
                {
                    DeleteFileIfExists(file);
                    continue;
                }

                // Also delete hand-written extension files that will be replaced
                // (e.g., DockerExtensions.cs will be replaced by DockerExtensions.Generated.cs)
                var expectedExtensionsFile = $"{namespacePrefix}Extensions.cs";
                if (fileName.Equals(expectedExtensionsFile, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Removing hand-written {File} - will be replaced by generated version", fileName);
                    DeleteFileIfExists(file);
                }
            }
        }

        _logger.LogInformation("Cleaned up old files matching prefix '{Prefix}' in {Path}", namespacePrefix, toolPath);
    }

    /// <summary>
    /// Checks if a file name matches the given namespace prefix.
    /// Handles special cases like "Extensions" suffix (e.g., "DockerExtensions.cs" matches "Docker")
    /// and interface naming (e.g., "IDocker.cs" matches "Docker").
    /// </summary>
    private static bool FileMatchesNamespacePrefix(string fileName, string namespacePrefix)
    {
        // Direct prefix match (e.g., "HelmInstallOptions.Generated.cs" matches "Helm")
        if (fileName.StartsWith(namespacePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Check for interface naming pattern (e.g., "IDocker.cs" or "IDocker.Generated.cs" should match "Docker")
        // The generator creates I{Prefix}.Generated.cs files
        var interfaceName = $"I{namespacePrefix}";
        if (fileName.StartsWith(interfaceName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Check for Extensions class naming pattern (e.g., "DockerExtensions.cs" should match "Docker")
        // The generator creates {Prefix}Extensions.cs files
        var extensionsName = $"{namespacePrefix}Extensions";
        if (fileName.StartsWith(extensionsName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a file contains the auto-generated header marker.
    /// </summary>
    private static bool IsAutoGeneratedFile(string filePath)
    {
        try
        {
            // Read first few lines to check for auto-generated marker
            using var reader = new StreamReader(filePath);
            for (var i = 0; i < 5 && !reader.EndOfStream; i++)
            {
                var line = reader.ReadLine();
                if (line != null && line.Contains("<auto-generated>", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void DeleteFileIfExists(string file)
    {
        try
        {
            File.Delete(file);
            _logger.LogDebug("Deleted old generated file: {File}", file);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete file: {File}", file);
        }
    }
}

/// <summary>
/// Result of the code generation process.
/// </summary>
public class GenerationResult
{
    public List<string> ToolsProcessed { get; } = [];
    public List<string> FilesGenerated { get; } = [];
    public List<ScrapingError> Errors { get; } = [];

    public bool HasErrors => Errors.Count > 0;

    public string GetSummary()
    {
        return $"""
            Generation Summary:
            - Tools processed: {ToolsProcessed.Count} ({string.Join(", ", ToolsProcessed)})
            - Files generated: {FilesGenerated.Count}
            - Errors: {Errors.Count}
            """;
    }
}
