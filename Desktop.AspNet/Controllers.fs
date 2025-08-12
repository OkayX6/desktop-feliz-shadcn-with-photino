namespace Desktop.Controllers

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc
open Shared


[<ApiController>]
[<Route("api/[controller]")>] // This will make the route /api/mydata
type Counter(logger: ILogger<Counter>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() = {| Value = 10 |}

[<ApiController>]
[<Route("api/[controller]")>] // This will make the route /api/mydata
type SystemInfo(logger: ILogger<SystemInfo>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =
        {| Platform = Environment.OSVersion.Platform.ToString()
           Version = Environment.OSVersion.VersionString |}


// Need to read the docs: https://github.com/Zaid-Ajaj/Fable.Remoting
[<ApiController>]
[<Route("api/[controller]")>] // This will make the route /api/mydata
type Update(logger: ILogger<SystemInfo>) =
    inherit ControllerBase()

    [<HttpPost>]
    member _.Update() = ()
