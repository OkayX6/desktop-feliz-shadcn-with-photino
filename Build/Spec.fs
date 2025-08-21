module Spec

open EasyBuild.FileSystemProvider
open Fake.IO

[<Literal>]
let root = __SOURCE_DIRECTORY__ + "\\..\\"
type Files = AbsoluteFileSystem<root>

module Ops =
  [<Literal>]
  let Dev = "Dev"
  [<Literal>]
  let Restore = "Restore"
  [<Literal>]
  let Install = "Install"
  
  
module Args =
  let mutable suave = false
  let mutable quick = false
  let setArgs args =
    let contains arg =
      args |> Array.exists ((=) arg)
    let getValue arg =
      args
      |> Array.tryFindIndex ((=) arg)
      |> Option.map ((+) 1)
      |> Option.bind(fun idx ->
          Array.tryItem idx args
        )
    suave <- contains "--suave"
    quick <- contains "--quick"
