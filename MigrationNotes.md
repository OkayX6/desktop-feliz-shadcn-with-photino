## TODO

- [ ] Rename **Desktop.AspNet** to **Desktop** after it gets feature parity
- [x] remove WebPack after its tested
- [ ] migrate **README.md** content to make it meaningful
- [ ] migrate **package.json** after its tested and all scenarios are working
- [x] add .editorconfig

## Ongoing

- [x] I'm testing that `npm start:vite` works
  - [x] Migrate all code from UserInterface to UserInterface.Vite
    - [x] FSProj
    - [x] App.fs
    - [x] Main.fs
    - [x] Extensions.fs
    - [x] styles.scss -> global.css
  - [x] At the end, remove all mentions of UserInterface.Vite and migrate back to UserInterface
- [x] Migrate & test that `npm run clean` works -> useless
- [x] I'm testing that `npm run build:vite` works -> migrate to `npm run build`
- [x] Migrate `npm run test` (mocha is broken apparently)
