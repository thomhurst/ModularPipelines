using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace ModularPipelines.UnitTests.BuildTransitive;

public class BuildServerFileDefaultsTests
{
    private static readonly string TargetsPath =
        Path.Combine(AppContext.BaseDirectory, "BuildTransitive", "ModularPipelines.targets");

    [Test]
    public async Task Defaults_Follow_The_Target_Framework_Runtime()
    {
        var properties = await EvaluateAsync();

        var expectedChannel = $"{Environment.Version.Major}.0";
        using (Assert.Multiple())
        {
            await Assert.That(properties["ModularPipelinesDotNetVersion"]).IsEqualTo($"{expectedChannel}.x");
            await Assert.That(properties["ModularPipelinesDotNetSdkImage"])
                .IsEqualTo($"mcr.microsoft.com/dotnet/sdk:{expectedChannel}");
        }
    }

    [Test]
    public async Task Sdk_Image_Follows_An_Overridden_DotNet_Version()
    {
        var properties = await EvaluateAsync("-p:ModularPipelinesDotNetVersion=10.0.x");

        await Assert.That(properties["ModularPipelinesDotNetSdkImage"]).IsEqualTo("mcr.microsoft.com/dotnet/sdk:10.0");
    }

    [Test]
    public async Task Explicit_Sdk_Image_Is_Preserved()
    {
        var properties = await EvaluateAsync("-p:ModularPipelinesDotNetSdkImage=registry.example/sdk:custom");

        await Assert.That(properties["ModularPipelinesDotNetSdkImage"]).IsEqualTo("registry.example/sdk:custom");
    }

    [Test]
    public async Task Azure_Pipeline_Resolves_Preview_Sdks_For_Wildcard_Versions()
    {
        var pipeline = await GenerateAsync("AzurePipelines", "azure-pipelines.yml");

        using (Assert.Multiple())
        {
            await Assert.That(pipeline).Contains($"version: \"{Environment.Version.Major}.0.x\"");
            await Assert.That(pipeline).Contains("includePreviewVersions: true");
        }
    }

    [Test]
    public async Task GitHub_Workflow_Uses_The_Wildcard_Channel()
    {
        // actions/setup-dotnet includes preview and RC SDKs for A.B.x wildcards, so no quality input is needed.
        var workflow = await GenerateAsync("GitHubActions", Path.Combine(".github", "workflows", "modular-pipelines.yml"));

        await Assert.That(workflow).Contains($"dotnet-version: \"{Environment.Version.Major}.0.x\"");
    }

    private static async Task<string> GenerateAsync(string buildSystem, string relativeOutputPath)
    {
        var directory = Directory.CreateTempSubdirectory("modularpipelines-targets-");
        try
        {
            var projectPath = await WriteProjectAsync(directory.FullName, importCommonTasks: true);

            await RunMSBuildAsync(
                directory.FullName,
                projectPath,
                "-t:GenerateModularPipelinesBuildServerFiles",
                $"-p:ModularPipelinesBuildSystem={buildSystem}",
                "-p:ModularPipelinesTargetFramework=net11.0");

            return await File.ReadAllTextAsync(Path.Combine(directory.FullName, relativeOutputPath));
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static async Task<Dictionary<string, string>> EvaluateAsync(params string[] extraArguments)
    {
        var directory = Directory.CreateTempSubdirectory("modularpipelines-targets-");
        try
        {
            var projectPath = await WriteProjectAsync(directory.FullName, importCommonTasks: false);
            var standardOutput = await RunMSBuildAsync(
                directory.FullName,
                projectPath,
                ["-getProperty:ModularPipelinesDotNetVersion", "-getProperty:ModularPipelinesDotNetSdkImage", .. extraArguments]);

            using var json = JsonDocument.Parse(standardOutput);
            return json.RootElement.GetProperty("Properties")
                .EnumerateObject()
                .ToDictionary(property => property.Name, property => property.Value.GetString()!);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static async Task<string> WriteProjectAsync(string directory, bool importCommonTasks)
    {
        // Built with XML APIs so checkout paths containing XML special characters stay valid.
        var project = new XElement("Project");
        if (importCommonTasks)
        {
            // Registers the built-in tasks that GenerateModularPipelinesBuildServerFiles uses.
            project.Add(new XElement("Import", new XAttribute("Project", "$(MSBuildToolsPath)/Microsoft.Common.tasks")));
        }

        project.Add(new XElement("Import", new XAttribute("Project", TargetsPath)));

        var projectPath = Path.Combine(directory, "Evaluate.proj");
        await File.WriteAllTextAsync(projectPath, project.ToString());
        return projectPath;
    }

    private static async Task<string> RunMSBuildAsync(string workingDirectory, string projectPath, params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = workingDirectory,
        };
        foreach (var argument in (string[]) ["msbuild", projectPath, "-nologo", .. arguments])
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)!;
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        var standardError = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"dotnet msbuild exited with {process.ExitCode}: {await standardOutput}{await standardError}");
        }

        return await standardOutput;
    }
}
