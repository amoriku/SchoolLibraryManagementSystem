import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Navigate, Route, Routes } from 'react-router'
import SignIn from './pages/auth/SignIn.tsx'
import App from './pages/home/App.tsx'
import './index.css'
import AdminPanel from './pages/panels/AdminPanel.tsx'
import BookCrud from './components/book/BookCrud.tsx'
import { Toaster } from 'react-hot-toast'
import AuthorCrud from './components/author/AuthorCrud.tsx'
import { AuthProvider } from './hooks/useAuth.tsx'

createRoot(document.getElementById('root')!).render(
  <>
    <Toaster position="bottom-right"></Toaster>
    <StrictMode>
      <BrowserRouter>
        <AuthProvider>

          <Routes>
            <Route path="/" element={<Navigate to="/home" replace />}></Route>
            <Route path="/home" element={<App />}></Route>
            <Route path="/sign-in" element={<SignIn />}></Route>
            <Route path="/sign-up" element={<Navigate to="/home" replace />}></Route>
            <Route path="/admin" element={<AdminPanel />}></Route>
            <Route path="/admin/books" element={<BookCrud />}></Route>
            <Route path="/admin/authors" element={<AuthorCrud />}></Route>
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </StrictMode>
  </>
)
