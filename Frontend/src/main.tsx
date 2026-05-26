import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Navigate, Route, Routes } from 'react-router'
import SignIn from './pages/auth/SignIn.tsx'
import App from './pages/home/App.tsx'
import './index.css'
import { Toaster } from 'react-hot-toast'
import { AuthProvider } from './hooks/useAuth.tsx'
import Hero from './components/Hero.tsx'
import { RootLayout } from './layouts/RootLayout.tsx'
import { AdminDashboard } from './components/dashboards/AdminDashboard.tsx'
import { UsersPage } from './pages/UsersPage.tsx'
import { LibrarianDashboard } from './components/dashboards/LibrarianDashboard.tsx'
import { ReadersPage } from './pages/ReadersPage.tsx'
import { BooksPage } from './pages/BooksPage.tsx'
import { DataProvider } from './hooks/useData.tsx'
import { AuthorsPage } from './pages/AuthorsPage.tsx'
import { ClassesPage } from './pages/ClassesPage.tsx'

createRoot(document.getElementById('root')!).render(
  <>
    <Toaster position="bottom-right" containerClassName='text-xl'></Toaster>
    <StrictMode>
      <BrowserRouter>
        <DataProvider>
          <AuthProvider>
            <Routes>
              <Route element={<RootLayout />}>
                <Route element={<Hero />}>
                  <Route path="/admin/dashboard" element={<AdminDashboard />}></Route>
                  <Route path="/admin/users" element={<UsersPage />}></Route>
                  <Route path="/admin/classes" element={<ClassesPage />}></Route>

                  <Route path="/librarian/dashboard" element={<LibrarianDashboard />}></Route>
                  <Route path="/librarian/books" element={<BooksPage />}></Route>
                  <Route path="/librarian/readers" element={<ReadersPage />}></Route>
                  <Route path="/librarian/authors" element={<AuthorsPage />}></Route>
                </Route>
              </Route>

              <Route path="/" element={<Navigate to="/home" replace />}></Route>
              <Route path="/home" element={<App />}></Route>
              <Route path="/login" element={<SignIn />}></Route>
              <Route path="/register" element={<Navigate to="/home" replace />}></Route>
              {/* <Route path="/admin" element={<AdminPanel />}></Route> */}
              {/* <Route path="/admin/books" element={<BookCrud />}></Route> */}
              {/* <Route path="/admin/authors" element={<AuthorCrud />}></Route> */}
            </Routes>
          </AuthProvider>
        </DataProvider>
      </BrowserRouter>
    </StrictMode>
  </>
)
