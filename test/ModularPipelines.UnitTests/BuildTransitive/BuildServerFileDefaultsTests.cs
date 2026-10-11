using System.Diagnostics;
using System.Text.Json;

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

    private static async Task<Dictionary<string, string>> EvaluateAsync(params string[] extraArguments)
    {
        var directory = Directory.CreateTempSubdirectory("modularpipelines-targets-");
        try
        {
            var projectPath = Path.Combine(directory.FullName, "Evaluate.proj");
            await File.WriteAllTextAsync(projectPath, $"""<Project><Import Project="{TargetsPath}" /></Project>""");

            var startInfo = new ProcessStartInfo("dotnet")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = directory.FullName,
            };
            foreach (var argument in (string[]) ["msbuild", projectPath, "-getProperty:ModularPipelinesDotNetVersion", "-getProperty:ModularPipelinesDotNetSdkImage", .. extraArguments])
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

            using var json = JsonDocument.Parse(await standardOutput);
            return json.RootElement.GetProperty("Properties")
                .EnumerateObject()
                .ToDictionary(property => property.Name, property => property.Value.GetString()!);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
