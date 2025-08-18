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
  let refreshKey, setRefreshKey = React.useState 0
  let counter = React.useDeferred (Server.Api.Counter(), [| refreshKey |])

  let counterValue =
    match counter with
    | Deferred.HasNotStartedYet -> "-"
    | Deferred.InProgress -> "loading..."
    | Deferred.Resolved counter -> $"{counter}"
    | Deferred.Failed _ -> "error"

  match data with
  | Deferred.HasNotStartedYet -> Html.none
  | Deferred.InProgress -> Html.h1 "Loading system info"
  | Deferred.Failed error -> Html.span error.Message
  | Deferred.Resolved system ->
    Html.div [
      prop.className "p-4 flex flex-col gap-2"
      prop.children [
        Html.h1 [
          prop.className "scroll-m-20 text-4xl font-extrabold tracking-tight text-balance"
          prop.text $"Platform: {system.Platform}"
        ]
        Html.h3 [
          prop.className "scroll-m-20 text-2xl font-semibold tracking-tight"
          prop.text $"Version: {system.Version}"
          prop.style [ style.color.mediumAquamarine ]
        ]

        Html.p [ Html.span "Counter value: "; Html.text counterValue ]

        Shadcn.button [
          prop.className "w-fit"
          prop.onClick (fun _ ->
            async {
              do! Server.Api.Update()
              setRefreshKey (refreshKey + 1)
            }
            |> Async.Ignore
            |> Async.StartImmediate)
          prop.text "Click me"
        ]
      ]
    ]
