module App

open Feliz
open Feliz.UseDeferred
open Feliz.Shadcn

/// <summary>
/// A React component that loads the system information from the backend API and shows on screen
/// </summary>
[<ReactComponent>]
let SystemInfo () =
  let data = React.useDeferred (Server.Api.SystemInfo(), [||])

  match data with
  | Deferred.HasNotStartedYet -> Html.none
  | Deferred.InProgress -> Html.h1 "Loading system info"
  | Deferred.Failed error -> Html.span error.Message
  | Deferred.Resolved system ->
    Html.div [ Html.h1 [ prop.className "scroll-m-20 text-4xl font-extrabold tracking-tight text-balance"
                         prop.text $"Platform: {system.Platform}" ]
               Html.h3 [ prop.className "scroll-m-20 text-2xl font-semibold tracking-tight"
                         prop.text $"Version: {system.Version}"
                         prop.style [ style.color.mediumAquamarine ] ]

               Shadcn.button [ prop.text "Show Alert"
                               prop.onClick (fun _ -> Browser.Dom.window.alert "Hello, shadcn/ui!") ]

               Shadcn.button [ prop.onClick (fun _ -> Server.Api.Update() |> Async.Ignore |> Async.StartImmediate)
                               prop.text "Click me" ] ]
