namespace Desktop.AspNet

#nowarn "20"

open System
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Fable.Remoting.AspNetCore
open Shared

module Program =
  open PhotinoNET
  open Fable.Remoting.Server
  open System.Net.Sockets
  open System.Net

  let serverApi: IServerApi =
    let mutable count = 0

    { Counter = fun () -> async { return count }

      SystemInfo =
        fun () ->
          async {
            return
              { Platform = Environment.OSVersion.Platform.ToString()
                Version = Environment.OSVersion.VersionString }
          }

      Update =
        fun () ->
          async {
            count <- count + 1
            return ()
          } }

  let exitCode = 0


  let isDevelopment =
#if DEBUG
    true
#else
    false
#endif

  let randomPort =
    let tcpListener = new TcpListener(IPAddress.Loopback, 0)
    tcpListener.Start()
    let port = (tcpListener.LocalEndpoint :?> IPEndPoint).Port
    tcpListener.Stop()
    port

  let desktopUrl =
    if
      isDevelopment
    // during development assume vite dev server is running
    then
      "http://localhost:5173"
    // in release mode we run Suave on a random port
    // which will host the static files generated
    else
      $"http://localhost:{randomPort}/index.html"

  let webApp =
    Remoting.createApi ()
    |> Remoting.fromValue serverApi
    // It's not in the documentation but if you don't do it, it doesn't work
    |> Remoting.withRouteBuilder routerPaths

  [<EntryPoint; STAThread>]
  let main args =
    let builder = WebApplication.CreateBuilder(args)

    // Configure the server to run on port 5000
    builder.WebHost.UseUrls("http://localhost:5000")

    let app = builder.Build()

    // Add Remoting handler to the ASP.NET Core pipeline
    app.UseRemoting(webApp)

    Task.Run(fun () -> app.Run()) |> ignore

    let window = new PhotinoWindow(Title = "Full Stack F# on Desktop (Using Photino)")

    window.Center().Load(Uri(desktopUrl)).WaitForClose()

    exitCode
