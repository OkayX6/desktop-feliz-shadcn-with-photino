namespace Desktop.AspNet

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc


module Program =
    open PhotinoNET
    open System.Text.Json

    let exitCode = 0

    let desktopUrl = "http://localhost:8080"
    // if isDevelopment
    // // during development assume webpack dev server is running
    // then "http://localhost:8080"
    // // in release mode we run Suave on a random port
    // // which will host the static files generated
    // else $"http://localhost:{randomPort}/index.html"

    [<EntryPoint; STAThread>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder
            .Services
            .AddControllers()
            .AddJsonOptions(fun options -> options.JsonSerializerOptions.PropertyNamingPolicy <- null)

        let app = builder.Build()

        app.UseAuthorization()
        app.MapControllers()

        Task.Run(fun () -> app.Run()) |> ignore

        let window = new PhotinoWindow(Title = "Full Stack F# on Desktop (Using Photino)")

        window
            .Center()
            .Load(Uri(desktopUrl))
            .WaitForClose()

        exitCode
