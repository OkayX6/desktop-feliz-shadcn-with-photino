open System
open System.IO
open Fake.IO
open Fake.Core
open Tools
open System.Text
open Spec

initializeContext()

let solutionRoot = Files.``App.sln``
let desktop = Files.Desktop.``.``
let clientDist = Path.combine Files.``.`` "dist"

Target.create Ops.Install <| fun _arg ->
  run dotnet [ "tool"; "install"; "fable"; "--create-manifest-if-needed" ] Files.``.``
  Fake.JavaScript.Npm.install (fun p ->
    { p with
        WorkingDirectory = Files.``.`` })

Target.create Ops.Restore <| fun _arg ->
  if not Args.quick then
    run dotnet [ "tool"; "restore" ] Files.``.``

Target.create Ops.Dev <| fun _arg ->
  [
    "vite", vite [] Files.``.``
    "fable", dotnet [ "tool"; "run"; "fable"; "watch" ] Files.UserInterface.``.``
    if Args.suave then
      "suave", dotnet [ "run" ] Files.Desktop.``.``
    else
      "aspnet", dotnet [ "run" ] Files.``Desktop.AspNet``.``.``
  ]
  |> runParallel

open Fake.Core.TargetOperators
let dependencies = [
  Ops.Restore
  ==> Ops.Dev
]
[<EntryPoint>]
let main args =
  Args.setArgs args
  args[0] |> Target.runOrDefaultWithArguments
  0
