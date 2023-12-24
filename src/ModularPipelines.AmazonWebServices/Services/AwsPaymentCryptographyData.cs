using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPaymentCryptographyData
{
    public AwsPaymentCryptographyData(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DecryptData(AwsPaymentCryptographyDataDecryptDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EncryptData(AwsPaymentCryptographyDataEncryptDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateCardValidationData(AwsPaymentCryptographyDataGenerateCardValidationDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateMac(AwsPaymentCryptographyDataGenerateMacOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GeneratePinData(AwsPaymentCryptographyDataGeneratePinDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReEncryptData(AwsPaymentCryptographyDataReEncryptDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TranslatePinData(AwsPaymentCryptographyDataTranslatePinDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyAuthRequestCryptogram(AwsPaymentCryptographyDataVerifyAuthRequestCryptogramOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyCardValidationData(AwsPaymentCryptographyDataVerifyCardValidationDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyMac(AwsPaymentCryptographyDataVerifyMacOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyPinData(AwsPaymentCryptographyDataVerifyPinDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}