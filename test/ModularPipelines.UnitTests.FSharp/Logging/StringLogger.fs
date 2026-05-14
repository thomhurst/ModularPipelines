namespace ModularPipelines.UnitTests.FSharp.Logging

open System
open System.Text
open Microsoft.Extensions.Logging

type StringLogger<'T>(stringBuilder: StringBuilder) =
    interface ILogger<'T> with
        member _.Log<'TState>(logLevel: LogLevel, eventId: EventId, state: 'TState, 
                             ex: Exception, formatter: Func<'TState, Exception, string>) =
            let log = formatter.Invoke(state, ex)
            stringBuilder.AppendLine(log) |> ignore

        member _.IsEnabled(logLevel: LogLevel) = true

        member _.BeginScope<'TState when 'TState: not null>(_: 'TState) : IDisposable =
            { new IDisposable with member _.Dispose() = () }
