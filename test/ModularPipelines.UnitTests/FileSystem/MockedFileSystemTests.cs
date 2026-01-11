using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class MockedFileSystemTests
{
    [Test]
    public async Task Module_CanUseMockedFileSystem_ForReading()
    {
        // Arrange - create mock provider that returns fake config
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("{\"enabled\": true}");

        // Act - run pipeline with mock provider
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                // Replace the default provider with our mock
                services.AddSingleton<IFileSystemProvider>(mockProvider.Object);
            })
            .AddModule<ConfigReaderModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Modules).HasCount().EqualTo(1);
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        // Verify module result is correct
        var moduleResults = await result.GetModuleResultsAsync();
        await Assert.That(moduleResults.First().ModuleResultType).IsEqualTo(ModuleResultType.Success);

        // Verify the module read from the mocked file
        mockProvider.Verify(p => p.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Module_CanUseMockedFileSystem_ForWriting()
    {
        // Arrange
        var mockProvider = new Mock<IFileSystemProvider>();

        // Act
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IFileSystemProvider>(mockProvider.Object);
            })
            .AddModule<FileWriterModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        // Verify write was called with expected content
        mockProvider.Verify(p => p.WriteAllTextAsync(
            It.IsAny<string>(),
            "Hello from module!",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Module_CanUseMockedFileSystem_ForFolderOperations()
    {
        // Arrange
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));

        // Act
        var result = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IFileSystemProvider>(mockProvider.Object);
            })
            .AddModule<FolderCreatorModule>()
            .ExecutePipelineAsync();

        // Assert
        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    // Test module that reads a config file
    private class ConfigReaderModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var configFile = context.FileSystem.GetFile("/config/settings.json");
            return await configFile.ReadAsync(cancellationToken);
        }
    }

    // Test module that writes to a file
    private class FileWriterModule : Module<bool?>
    {
        public override async Task<bool?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var outputFile = context.FileSystem.GetFile("/output/result.txt");
            await outputFile.WriteAsync("Hello from module!", cancellationToken);
            return true;
        }
    }

    // Test module that creates a folder
    private class FolderCreatorModule : Module<bool?>
    {
        public override Task<bool?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var folder = context.FileSystem.GetFolder("/data/output");
            folder.Create();
            return Task.FromResult<bool?>(true);
        }
    }
}
