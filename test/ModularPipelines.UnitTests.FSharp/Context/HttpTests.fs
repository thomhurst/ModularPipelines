namespace ModularPipelines.UnitTests.FSharp.Context

open System
open System.IO
open System.Net.Http
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open ModularPipelines.Http
open ModularPipelines.Options
open ModularPipelines.TestHelpers
open NReco.Logging.File
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open Vertical.SpectreLogger.Options

type HttpTests() =
    inherit TestBase()

    member private this.GetHttpWithFileLogger(file: string) : Task<struct (IHttp * ModularPipelines.IPipeline)> =
        this.GetService<IHttp>(
            Action<IServiceCollection>(fun services ->
                services.AddLogging(fun builder ->
                    services.Configure<SpectreLoggerOptions>(fun (options: SpectreLoggerOptions) ->
                        options.MinimumLogLevel <- LogLevel.Information)
                    |> ignore

                    services.Configure<LoggerFilterOptions>(fun (options: LoggerFilterOptions) ->
                        options.MinLevel <- LogLevel.Information)
                    |> ignore

                    builder.AddFile(file) |> ignore)
                |> ignore)
        )

    [<Test>]
    member this.Can_Send_Request_With_String_To_Request_Implicit_Conversion() = async {
        let! http = this.GetService<IHttp>() |> Async.AwaitTask
        use! response = http.SendAsync(Uri("https://thomhurst.github.io/TUnit")) |> Async.AwaitTask
        do! check(Assert.That(response.IsSuccessStatusCode).IsTrue())
    }

    [<Test>]
    member this.When_Log_Request_False_Then_Do_Not_Log_Request() = async {
        let file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt")

        try
            let! struct (http, host) = this.GetHttpWithFileLogger(file) |> Async.AwaitTask

            do!
                http.SendAsync(
                    HttpOptions(
                        HttpRequestMessage(HttpMethod.Get, Uri("https://thomhurst.github.io/TUnit")),
                        ThrowOnNonSuccessStatusCode = false,
                        LoggingType = HttpLoggingType.Response
                    )
                )
                |> Async.AwaitTask
                |> Async.Ignore

            do! host.DisposeAsync().AsTask() |> Async.AwaitTask

            let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
            do! check(Assert.That(not (logFile.Contains("HTTP Request:"))).IsTrue())
            do! check(Assert.That(not (logFile.Contains("GET https://thomhurst.github.io/TUnit HTTP/1.1"))).IsTrue())
            do! check(Assert.That(logFile.Contains("HTTP Response:")).IsTrue())
            do! check(Assert.That(logFile.Contains("Server: GitHub.com")).IsTrue())
        finally
            if File.Exists(file) then
                File.Delete(file)
    }

    [<Test>]
    member this.When_Log_Response_False_Then_Do_Not_Log_Response() = async {
        let file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt")

        try
            let! struct (http, host) = this.GetHttpWithFileLogger(file) |> Async.AwaitTask

            do!
                http.SendAsync(
                    HttpOptions(
                        HttpRequestMessage(HttpMethod.Get, Uri("https://thomhurst.github.io/TUnit")),
                        ThrowOnNonSuccessStatusCode = false,
                        LoggingType = HttpLoggingType.Request
                    )
                )
                |> Async.AwaitTask
                |> Async.Ignore

            do! host.DisposeAsync().AsTask() |> Async.AwaitTask

            let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
            do! check(Assert.That(logFile.Contains("HTTP Request:")).IsTrue())
            do! check(Assert.That(logFile.Contains("GET https://thomhurst.github.io/TUnit HTTP/1.1")).IsTrue())
            do! check(Assert.That(not (logFile.Contains("HTTP Response:"))).IsTrue())
            do! check(Assert.That(not (logFile.Contains("Server: GitHub.com"))).IsTrue())
        finally
            if File.Exists(file) then
                File.Delete(file)
    }

    [<Test>]
    [<Arguments(true)>]
    [<Arguments(false)>]
    member this.Assert_LoggingHttpClient_Logs_As_Expected(customHttpClient: bool) = async {
        let file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt")

        try
            let! struct (http, host) = this.GetHttpWithFileLogger(file) |> Async.AwaitTask

            if customHttpClient then
                use customClient = new HttpClient()

                do!
                    http.SendAsync(
                        HttpOptions(
                            HttpRequestMessage(HttpMethod.Get, Uri("https://thomhurst.github.io/TUnit")),
                            ThrowOnNonSuccessStatusCode = false,
                            HttpClient = customClient
                        )
                    )
                    |> Async.AwaitTask
                    |> Async.Ignore
            else
                use! response = http.GetLoggingHttpClient().GetAsync(Uri("https://thomhurst.github.io/TUnit")) |> Async.AwaitTask
                ()

            do! host.DisposeAsync().AsTask() |> Async.AwaitTask

            let! logFile = File.ReadAllTextAsync(file) |> Async.AwaitTask
            let! logFileLines = File.ReadAllLinesAsync(file) |> Async.AwaitTask
            let indexOfRequest = logFileLines |> Array.findIndex (fun line -> line.Contains("HTTP Request:"))
            let indexOfStatusCode = logFileLines |> Array.findIndex (fun line -> line.Contains("HTTP Status:"))
            let indexOfDuration = logFileLines |> Array.findIndex (fun line -> line.Contains("Duration:"))
            let indexOfResponse = logFileLines |> Array.findIndex (fun line -> line.Contains("HTTP Response:"))

            do! check(Assert.That(logFile.Contains("HTTP Request:")).IsTrue())
            do! check(Assert.That(logFile.Contains("GET https://thomhurst.github.io/TUnit HTTP/1.1")).IsTrue())
            do! check(Assert.That(logFile.Contains("HTTP Response:")).IsTrue())
            do! check(Assert.That(logFile.Contains("Headers")).IsTrue())
            do! check(Assert.That(logFile.Contains("Server: GitHub.com")).IsTrue())
            do! check(Assert.That(logFile.Contains("Body")).IsTrue())
            do! check(Assert.That(logFile.Contains("Duration:")).IsTrue())
            do! check(Assert.That(logFile.Contains("HTTP Status:")).IsTrue())
            do! check(Assert.That(indexOfRequest < indexOfStatusCode).IsTrue())
            do! check(Assert.That(indexOfStatusCode < indexOfDuration).IsTrue())
            do! check(Assert.That(indexOfDuration < indexOfResponse).IsTrue())
        finally
            if File.Exists(file) then
                File.Delete(file)
    }
