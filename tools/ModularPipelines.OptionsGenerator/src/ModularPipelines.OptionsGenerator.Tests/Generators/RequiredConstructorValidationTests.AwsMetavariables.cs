using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Aws_Log_Tail_Metavariables_Preserve_Constructors_And_Optional_Filters()
    {
        var scraper = new AwsCliScraper(new AwsLogTailExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Count).IsEqualTo(2);
        var tool = scraper.CreateToolDefinition() with
        {
            TargetNamespace = "ModularPipelines.Tool",
            Commands = [.. commands.Select(command => command with { ParentClassName = "ToolOptions" })],
        };
        var generated = await new OptionsClassGenerator().GenerateAsync(tool);
        var assembly = Compile([.. generated.Select(file => file.Content)]);
        foreach (var command in commands)
        {
            var type = assembly.GetType($"ModularPipelines.Tool.Options.{command.ClassName}")!;
            await Assert.That(type.GetProperty("Value")).IsNull();
            object input = command.CommandParts[^1] == "tail" ? "group" : new[] { "group" };
            var instance = Activator.CreateInstance(type, [input])!;
            foreach (var names in new string[]?[] { null, [], ["stream-a", "stream-b"] })
            {
                type.GetProperty("LogStreamNames")!.SetValue(instance, names);
                var results = new List<ValidationResult>();
                await Assert.That(Validator.TryValidateObject(instance, new(instance), results, true)).IsTrue();
            }

            type.GetProperty("LogStreamNames")!.SetValue(instance, null);
            type.GetProperty("LogStreamNamePrefixes")?.SetValue(instance, value);
            await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsTrue();
        }
    }

    private static readonly string[] value = new[] { "prefix" };

    // Synopsis and option type excerpts from AWS logs tail/start-live-tail help.
    // Their generic <value> notation is shared by AWS CLI 2.32.28 and 2.36.46.
    private sealed class AwsLogTailExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "help" => "AVAILABLE SERVICES\n       o logs",
                    "logs help" => "AVAILABLE COMMANDS\n       o tail\n       o start-live-tail",
                    "logs tail help" => """
                        SYNOPSIS
                               tail
                               group_name <value>
                               [--log-stream-names <value> [<value>...]]
                               [--log-stream-name-prefix <value>]
                        OPTIONS
                               group_name (string)
                                The name of the CloudWatch Logs group.
                               --log-stream-names (string)
                                The list of stream names to filter logs by.
                               --log-stream-name-prefix (string)
                                The prefix to filter logs by.
                        """,
                    "logs start-live-tail help" => """
                        SYNOPSIS
                               start-live-tail
                               --log-group-identifiers <value> [<value>...]
                               [--log-stream-names <value> [<value>...]]
                               [--log-stream-name-prefixes <value> [<value>...]]
                        OPTIONS
                               --log-group-identifiers (list) [required]
                                The Log Group Identifiers are the ARNs for the groups to tail.
                               --log-stream-names (list)
                                The list of stream names to filter logs by.
                               --log-stream-name-prefixes (list)
                                The prefixes to filter logs by.
                        """,
                    _ => string.Empty,
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
