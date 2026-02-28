import { Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './hooks/useAuth.js';
import ProtectedRoute from './components/auth/ProtectedRoute.js';
import AppLayout from './components/layout/AppLayout.js';
import OrdersPage from './pages/order/OrderPage.js';
import AuthPage from './pages/auth/AuthPage.js';

const App = () => {
  return (
    <AuthProvider>
      <Routes>
        <Route path='/auth' element={<AuthPage />} />
        <Route path='/' element={<Navigate to="/order" replace />} />
        <Route
          path="/*"
          element={
            <ProtectedRoute>
              <AppLayout>
                <Routes>
                  <Route path='/order' element={<OrdersPage />} />
                </Routes>
              </AppLayout>
            </ProtectedRoute>
          } />
      </Routes>
    </AuthProvider>
  );
};

export default App;
