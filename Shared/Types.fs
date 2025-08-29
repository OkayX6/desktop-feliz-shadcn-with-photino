module Shared

/// Defines how routes are generated on server and mapped from client
let routerPaths typeName method = $"/api/%s{method}"

type DirectoryContent = {
  DirectoryPath: string
  Files: string[]
  Directories: string[]
}

/// A type that specifies the communication protocol between client and server
/// to learn more, read the docs at https://zaid-ajaj.github.io/Fable.Remoting/src/basics.html
type IServerApi = {
  CurrentFiles: unit -> Async<DirectoryContent>
  MoveUp: unit -> Async<unit>
  GoToDirectory: string -> Async<unit>
  Reset: unit -> Async<unit> }
