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

        // Run-wide record of emitted paths, so tools sharing an output directory
        // (e.g. kubectl and kustomize in ModularPipelines.Kubernetes) can't silently
        // overwrite each other's generated files.
        var emittedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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

                // Try CLI scraper first if enabled
                if (useCliFirst && cliScrapersByTool.TryGetValue(htmlScraper.ToolName, out var cliScraper))
                {
                    var cliFailureReason = await GenerateFromCliAsync(cliScraper, outputDirectory, result, emittedPaths, cancellationToken);
                    if (cliFailureReason is null)
                    {
                        continue;
                    }

                    _logger.LogWarning("{Reason} Falling back to HTML scraper.", cliFailureReason);
                }

                // Fall back to HTML scraper (batch mode)
                _logger.LogInformation("Using HTML scraper for {Tool}", htmlScraper.ToolName);

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

                await GenerateForToolAsync(htmlToolDefinition, outputDirectory, result, emittedPaths, cancellationToken);

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
                    processedTools.Add(cliScraper.ToolName);
                    result.ToolsProcessed.Add(cliScraper.ToolName);

                    var failureReason = await GenerateFromCliAsync(cliScraper, outputDirectory, result, emittedPaths, cancellationToken);
                    if (failureReason is not null)
                    {
                        throw new InvalidOperationException(
                            $"{failureReason} This is a CLI-only tool with no HTML fallback.");
                    }
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

    /// <summary>
    /// Scrapes a tool via its CLI scraper and generates code for it.
    /// Returns null when generation completed, or a failure reason when it could not run.
    /// Failure policy belongs to the caller: the HTML-scraper loop falls back, the
    /// CLI-only loop throws.
    /// </summary>
    private async Task<string?> GenerateFromCliAsync(
        ICliScraper cliScraper,
        string outputDirectory,
        GenerationResult result,
        HashSet<string> emittedPaths,
        CancellationToken cancellationToken)
    {
        if (!await cliScraper.IsAvailableAsync(cancellationToken))
        {
            return $"The {cliScraper.ToolName} CLI is not available on PATH - check the tool's install step.";
        }

        _logger.LogInformation("Using CLI scraper for {Tool}", cliScraper.ToolName);

        var toolDefinition = cliScraper.CreateToolDefinition();
        var allCommands = new List<CliCommandDefinition>();

        await foreach (var command in cliScraper.ScrapeAsync(cancellationToken))
        {
            allCommands.Add(command);
        }

        _logger.LogInformation("Scraped {Count} commands for {Tool}", allCommands.Count, cliScraper.ToolName);

        if (allCommands.Count == 0)
        {
            // The CLI is present (IsAvailableAsync passed), so "not installed" is not the
            // problem - the scraper failed to parse anything out of the help output.
            return $"The {cliScraper.ToolName} CLI reported itself available but help scraping produced no commands. " +
                   "Check the scraper's help-text parsing against the currently installed CLI version.";
        }

        var completeToolDefinition = new CliToolDefinition
        {
            ToolName = toolDefinition.ToolName,
            NamespacePrefix = toolDefinition.NamespacePrefix,
            TargetNamespace = toolDefinition.TargetNamespace,
            OutputDirectory = toolDefinition.OutputDirectory,
            Commands = allCommands,
            Errors = []
        };

        await GenerateForToolAsync(completeToolDefinition, outputDirectory, result, emittedPaths, cancellationToken);

        _logger.LogInformation("Generated files for {Tool} ({Count} commands, {SubDomainCount} sub-domains)",
            cliScraper.ToolName, allCommands.Count, completeToolDefinition.SubDomainGroups.Count);

        return null;
    }

    /// <summary>
    /// Runs all generators for a tool and writes the results to disk.
    /// All files are generated in memory and validated for path collisions before anything
    /// on disk is touched, so a scraping or generation failure never mutates the package.
    /// Writes happen before stale files are pruned, so nothing is ever deleted until its
    /// replacement exists on disk - an I/O failure mid-write leaves the old files in
    /// place (possibly alongside new ones, which the compile check then catches).
    /// </summary>
    private async Task GenerateForToolAsync(
        CliToolDefinition tool,
        string outputDirectory,
        GenerationResult result,
        HashSet<string> emittedPaths,
        CancellationToken cancellationToken)
    {
        var normalizedCommands = GeneratorUtils.NormalizeCommandClassNames(tool.Commands);
        var toolDefinition = ReferenceEquals(normalizedCommands, tool.Commands)
            ? tool
            : tool with { Commands = normalizedCommands };

        var generatedFiles = new List<GeneratedFile>();

        foreach (var generator in _generators)
        {
            generatedFiles.AddRange(await generator.GenerateAsync(toolDefinition, cancellationToken));
        }

        GeneratorUtils.EnsureNoDuplicateFilePaths(generatedFiles, emittedPaths);

        var writtenFullPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in generatedFiles)
        {
            var fullPath = Path.Combine(outputDirectory, file.RelativePath);
            await WriteFileAsync(fullPath, file.Content, cancellationToken);
            writtenFullPaths.Add(Path.GetFullPath(fullPath));
            result.FilesGenerated.Add(file.RelativePath);
            emittedPaths.Add(file.RelativePath.Replace('\\', '/'));
        }

        // Only after every replacement is on disk is it safe to prune stale files.
        CleanupGeneratedFiles(outputDirectory, toolDefinition.OutputDirectory, toolDefinition.NamespacePrefix, writtenFullPaths);

        // AssemblyInfo is deliberately outside collision tracking. Its metadata is
        // package-level, so tools sharing an output directory generate identical content.
        await WriteAssemblyInfoAsync(outputDirectory, toolDefinition, cancellationToken);
        result.FilesGenerated.Add(Path.Combine(toolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs"));
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
    /// This centralizes generation metadata in one file per assembly rather than in every
    /// generated file, reducing unnecessary diffs when regenerating unchanged files.
    /// </summary>
    private static async Task WriteAssemblyInfoAsync(
        string outputDirectory,
        CliToolDefinition toolDefinition,
        CancellationToken cancellationToken)
    {
        var content = GeneratorUtils.GenerateAssemblyInfo(toolDefinition.TargetNamespace);
        var path = Path.Combine(outputDirectory, toolDefinition.OutputDirectory, "AssemblyInfo.Generated.cs");
        await WriteFileAsync(path, content, cancellationToken);
    }

    /// <summary>
    /// Prunes stale generated files after the current generation has been written.
    /// Only deletes files that match the namespace prefix to avoid removing files from other tools
    /// that share the same output directory (e.g., kubectl and kustomize in ModularPipelines.Kubernetes).
    /// Files just written this run (<paramref name="pathsToKeep"/>) and files without the
    /// auto-generated header (hand-written) are never deleted.
    /// </summary>
    private void CleanupGeneratedFiles(
        string outputDirectory,
        string toolOutputDirectory,
        string namespacePrefix,
        IReadOnlySet<string> pathsToKeep)
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
                if (pathsToKeep.Contains(Path.GetFullPath(file)))
                {
                    continue; // Written this run - not stale
                }

                var fileName = Path.GetFileName(file);
                if (FileMatchesNamespacePrefix(fileName, namespacePrefix))
                {
                    DeleteFileIfExists(file);
                }
            }

            // Also clean up legacy files that were auto-generated before the .Generated.cs
            // naming convention. The auto-generated header marker is required - files
            // without it are hand-written and must never be deleted by the generator.
            var allCsFiles = Directory.GetFiles(subDirPath, "*.cs", SearchOption.TopDirectoryOnly);
            foreach (var file in allCsFiles)
            {
                if (file.EndsWith(".Generated.cs", StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Already handled above
                }

                var fileName = Path.GetFileName(file);

                if (FileMatchesNamespacePrefix(fileName, namespacePrefix) && IsAutoGeneratedFile(file))
                {
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
