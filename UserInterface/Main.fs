module Main

open Feliz
open Browser.Dom
open Fable.Core.JsInterop

importAll "./styles/global.css"

let root = ReactDOM.createRoot (document.getElementById "feliz-app")
root.render (App.Root())
