import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
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
  }
});
