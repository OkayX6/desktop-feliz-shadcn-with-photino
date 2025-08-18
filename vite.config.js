import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from '@tailwindcss/vite'
import path from "path"

export default defineConfig({
  plugins: [react(), tailwindcss()],
  root: "./UserInterface",
  build: {
    outDir: "../dist",
  },
  // Only in dev mode
  // This allows us to redirect /api urls to the ASP.Net server
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  },
  resolve: {
    alias: {
        "@": path.resolve(__dirname, "./UserInterface"),
    },
},
});
