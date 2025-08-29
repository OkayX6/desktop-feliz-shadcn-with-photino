module App

open Feliz
open Feliz.UseDeferred
open Feliz.Shadcn


[<ReactComponent>]
let DirectoryContent (directoryName: string, directories: string[], files: string[], refresh: unit -> unit) =
  Shadcn.table [
    Shadcn.tableHeader [ Shadcn.tableRow [ Shadcn.tableHead "Filesystem Entry" ] ]
    Shadcn.tableBody [
      for d in directories do
        Shadcn.tableRow [
          prop.key d
          prop.children [
            Shadcn.tableCell [
              prop.className "font-semibold italic flex items-center"
              prop.onClick (fun _ ->
                async {
                  let targetDirectory = $"{directoryName}\\{d}"
                  do! Server.Api.GoToDirectory(targetDirectory)
                  refresh ()
                }
                |> Async.StartImmediate)
              prop.children [ Lucide.Folder [ svg.className "mr-1 h-4 w-4 opacity-70" ]; Html.text d ]
            ]
          ]
        ]
      for file in files do
        Shadcn.tableRow [
          prop.key file
          prop.children [ Shadcn.tableCell [ prop.className ""; prop.text file ] ]
        ]
    ]
  ]


/// <summary>
/// A React component that loads the system information from the backend API and shows on screen
/// </summary>
[<ReactComponent>]
let Root () =
  let refreshKey, setRefreshKey = React.useState 0
  let data = React.useDeferred (Server.Api.CurrentFiles(), [|refreshKey|])
  let refresh () = setRefreshKey (refreshKey + 1)

  Html.div [
    prop.className "p-4 flex flex-col gap-2"
    prop.children [

      Html.h1 [
        prop.className "scroll-m-20 text-4xl font-extrabold tracking-tight text-balance"
        prop.text $"[Photino + Shadcn + F#] File Explorer"
      ]

      Html.div [
        prop.className "flex gap-2"
        prop.children [
          Shadcn.button [
            prop.className "w-fit"
            prop.onClick (fun _ ->
              async {
                do! Server.Api.MoveUp()
                return refresh () }
              |> Async.StartImmediate)
            prop.children [
              Lucide.ChevronUp []
              Html.text "Move Up"
            ]
          ]

          Shadcn.button [
            prop.className "w-fit"
            prop.onClick (fun _ ->
              async {
                do! Server.Api.Reset()
                refresh ()
              }
              |> Async.StartImmediate)
            prop.children [
              Lucide.RotateCcw []
              Html.text "Reset Directory"
            ]
          ]
        ]
      ]

      match data with
      | Deferred.HasNotStartedYet -> Html.none
      | Deferred.InProgress -> Html.h1 "Loading system info"
      | Deferred.Failed error -> Html.span error.Message
      | Deferred.Resolved content ->
        Html.h3 [
          prop.className "scroll-m-20 text-2xl font-semibold tracking-tight"
          prop.text $"Folder: {content.DirectoryPath}"
        ]

        DirectoryContent(content.DirectoryPath, content.Directories, content.Files, refresh)
    ]
  ]
