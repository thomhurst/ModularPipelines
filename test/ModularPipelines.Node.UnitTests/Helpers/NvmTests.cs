using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Node.Extensions;
using Moq;

namespace ModularPipelines.Node.UnitTests.Helpers;

public class NvmTests
{
    [Test]
    [Arguments("Windows", "use")]
    [Arguments("Windows", "install")]
    [Arguments("Windows", "version")]
    [Arguments("Windows", "current")]
    [Arguments("Linux", "use")]
    [Arguments("Linux", "install")]
    [Arguments("Linux", "version")]
    [Arguments("Linux", "current")]
    [Arguments("OSX", "use")]
    [Arguments("OSX", "install")]
    [Arguments("OSX", "version")]
    [Arguments("OSX", "current")]
    public async Task Dispatches_Platform_Command_And_Propagates_Cancellation(string platform, string operation)
    {
        using var fixture = new Fixture(platform);
        using var cancellation = new CancellationTokenSource();
        const string version = "v22 with 'quotes' \"and\" $(touch injected); & stuff";

        Task<CommandResult> Invoke() => operation switch
        {
            "use" => fixture.Nvm.UseAsync(version, cancellation.Token),
            "install" => fixture.Nvm.InstallAsync(version, cancellation.Token),
            "version" => fixture.Nvm.VersionAsync(cancellation.Token),
            _ => fixture.Nvm.CurrentAsync(cancellation.Token),
        };

        await Invoke();
        var command = operation == "version" && platform != "Windows" ? "--version" : operation;
        string[] expected = operation is "use" or "install" ? [command, version] : [command];
        await Assert.That(fixture.Tool).IsEqualTo(platform == "Windows" ? "nvm" : "bash");
        await Assert.That(fixture.Token).IsEqualTo(cancellation.Token);
        await Assert.That(fixture.Arguments.Skip(platform == "Windows" ? 0 : 3).SequenceEqual(expected)).IsTrue();
        if (platform != "Windows")
        {
            await Assert.That(fixture.Arguments[0]).IsEqualTo("-c");
            await Assert.That(fixture.Arguments[2]).IsEqualTo("nvm");
            await Assert.That(fixture.Arguments[1].Contains('\r')).IsFalse();
            await Assert.That(fixture.Arguments[1].Contains(version, StringComparison.Ordinal)).IsFalse();
        }

        await cancellation.CancelAsync();
        var exception = await Assert.ThrowsAsync<OperationCanceledException>(Invoke);
        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
    }

    [Test]
    [BashAvailableTest]
    [Arguments("explicit")]
    [Arguments("xdg")]
    [Arguments("home")]
    public async Task Unix_Sources_Nvm_And_Preserves_Arguments_And_Paths(string location)
    {
        using var fixture = new Fixture("Linux");
        const string version = "v22 'quoted' \"double\" $(echo injected); & value";
        await fixture.Nvm.InstallAsync(version);
        var root = Path.Combine(Path.GetTempPath(), $"nvm test 'quotes' {Guid.NewGuid():N}");
        var directory = location switch
        {
            "explicit" => Path.Combine(root, "custom nvm"),
            "xdg" => Path.Combine(root, "config dir", "nvm"),
            _ => Path.Combine(root, ".nvm"),
        };
        Directory.CreateDirectory(directory);
        try
        {
            // A shell function, not an executable. Also check that sourcing cannot consume nvm's arguments.
            await File.WriteAllTextAsync(Path.Combine(directory, "nvm.sh"), """
                [ "$1" = '--no-use' ] || return 98
                nvm() { printf '%s\n' "$#" "$1" "$2"; }
                """.ReplaceLineEndings("\n"));
            var start = BashStartInfo(fixture.Arguments);
            start.Environment["NVM_DIR"] = location == "explicit" ? UnixPath(directory) : string.Empty;
            start.Environment["XDG_CONFIG_HOME"] = location == "home" ? string.Empty : UnixPath(Path.Combine(root, "config dir"));
            start.Environment["HOME"] = UnixPath(root);
            using var process = Process.Start(start)!;
            var output = process.StandardOutput.ReadToEndAsync();
            var error = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            await Assert.That(process.ExitCode).IsEqualTo(0);
            await Assert.That(await error).IsEmpty();
            await Assert.That((await output).Replace("\r\n", "\n", StringComparison.Ordinal)).IsEqualTo($"2\ninstall\n{version}\n");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    [BashAvailableTest]
    public async Task Unix_Does_Not_Run_Nvm_When_Sourcing_Fails()
    {
        using var fixture = new Fixture("Linux");
        await fixture.Nvm.CurrentAsync();
        var start = BashStartInfo(fixture.Arguments);
        start.Environment["NVM_DIR"] = "/nonexistent-nvm-" + Guid.NewGuid().ToString("N");
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsNotEqualTo(0);
        await Assert.That(await output).IsEmpty();
        await Assert.That(await error).Contains("nvm.sh");
    }

    private static string UnixPath(string path) => path.Replace('\\', '/');

    private static ProcessStartInfo BashStartInfo(IEnumerable<string> arguments)
    {
        var start = new ProcessStartInfo(OperatingSystem.IsWindows() ? "C:/Program Files/Git/bin/bash.exe" : "bash")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        start.Environment.Remove("BASH_ENV");
        foreach (var argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }

        return start;
    }

    private sealed class Fixture : IDisposable
    {
        private readonly ServiceProvider _provider;

        public Fixture(string platform)
        {
            var environment = new Mock<IEnvironmentContext>();
            environment.SetupGet(x => x.OperatingSystem).Returns(OSPlatform.Create(platform.ToUpperInvariant()));
            var shell = new Mock<IShellContext>(MockBehavior.Strict);
            shell.Setup(x => x.RunAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>(), It.IsAny<CancellationToken>()))
                .Returns((string tool, IReadOnlyList<string> arguments, CancellationToken token) =>
                {
                    Tool = tool;
                    Arguments = arguments;
                    Token = token;
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult(CommandResult.Ok("captured"));
                });
            var context = new Mock<IPipelineContext>();
            context.SetupGet(x => x.Environment).Returns(environment.Object);
            context.SetupGet(x => x.Shell).Returns(shell.Object);
            var services = new ServiceCollection();
            services.AddSingleton(context.Object);
            services.RegisterNodeContext();
            _provider = services.BuildServiceProvider();
            Nvm = _provider.GetRequiredService<INvm>();
        }

        public INvm Nvm { get; }

        public string? Tool { get; private set; }

        public IReadOnlyList<string> Arguments { get; private set; } = [];

        public CancellationToken Token { get; private set; }

        public void Dispose() => _provider.Dispose();
    }

    public class BashAvailableTestAttribute() : SkipAttribute("Requires native Bash or Git Bash on Windows")
    {
        public override Task<bool> ShouldSkip(TestRegisteredContext context)
        {
            return Task.FromResult(OperatingSystem.IsWindows() && !File.Exists("C:/Program Files/Git/bin/bash.exe"));
        }
    }
}
