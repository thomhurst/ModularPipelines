using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatamigration
{
    public AzDatamigration(
        AzDatamigrationSqlDb sqlDb,
        AzDatamigrationSqlManagedInstance sqlManagedInstance,
        AzDatamigrationSqlService sqlService,
        AzDatamigrationSqlVm sqlVm,
        ICommand internalCommand
    )
    {
        SqlDb = sqlDb;
        SqlManagedInstance = sqlManagedInstance;
        SqlService = sqlService;
        SqlVm = sqlVm;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDatamigrationSqlDb SqlDb { get; }

    public AzDatamigrationSqlManagedInstance SqlManagedInstance { get; }

    public AzDatamigrationSqlService SqlService { get; }

    public AzDatamigrationSqlVm SqlVm { get; }

    public async Task<CommandResult> GetAssessment(AzDatamigrationGetAssessmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationGetAssessmentOptions(), token);
    }

    public async Task<CommandResult> GetSkuRecommendation(AzDatamigrationGetSkuRecommendationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationGetSkuRecommendationOptions(), token);
    }

    public async Task<CommandResult> LoginMigration(AzDatamigrationLoginMigrationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationLoginMigrationOptions(), token);
    }

    public async Task<CommandResult> PerformanceDataCollection(AzDatamigrationPerformanceDataCollectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationPerformanceDataCollectionOptions(), token);
    }

    public async Task<CommandResult> RegisterIntegrationRuntime(AzDatamigrationRegisterIntegrationRuntimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SqlServerSchema(AzDatamigrationSqlServerSchemaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServerSchemaOptions(), token);
    }

    public async Task<CommandResult> TdeMigration(AzDatamigrationTdeMigrationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationTdeMigrationOptions(), token);
    }
}