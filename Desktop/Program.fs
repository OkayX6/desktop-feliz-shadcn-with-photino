namespace Desktop

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
  open Photino.NET
  open Fable.Remoting.Server
  open System.Net.Sockets
  open System.Net
  open System.Reflection
  open System.IO

  let serverApi: IServerApi =
    let initialDirectory = Assembly.GetExecutingAssembly().Location
    let mutable currentDirectory = initialDirectory

    { CurrentFiles =
        fun () ->
          async {
            return
              { DirectoryPath = currentDirectory
                Files = Directory.EnumerateFiles(currentDirectory) |> Seq.toArray
                Directories =
                  Directory.EnumerateDirectories(currentDirectory)
                  |> Seq.toArray }
          }
      MoveUp =
        fun () ->
          async {
            let parentDir = Directory.GetParent(currentDirectory)

            if parentDir.Exists then
              currentDirectory <- parentDir.FullName
          }
      GoToDirectory =
        fun (dirPath: string) ->
          async {
            if Directory.Exists(dirPath) then
              currentDirectory <- dirPath
          }

      Reset = fun () -> async { currentDirectory <- initialDirectory } }

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

  let apiHostUrl =
    if isDevelopment then
      "http://localhost:5000"
    else
      $"http://localhost:{randomPort}"

  let desktopUrl =
    if
      isDevelopment
    // during development assume vite dev server is running
    then
      "http://localhost:5173"
    // in release mode we run ASP.NET Core on a random port
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
    // Configure the server hosting URL
    builder.WebHost.UseUrls(apiHostUrl)

    let app = builder.Build()

    do
      app.UseRemoting(webApp) // Add Fable.Remoting handler to the ASP.NET Core pipeline

      if not isDevelopment then
        app.UseStaticFiles() |> ignore // In release mode, serve static files to host the website

      Task.Run(fun () -> app.Run())

    let window = new PhotinoWindow(Title = "Full Stack F# on Desktop (Using Photino)")
    window.Center().Load(Uri(desktopUrl)).WaitForClose()

    exitCode
