import { Routes, Route, Navigate } from 'react-router-dom';
import { Spin } from 'antd';
import { AuthProvider, useAuth } from './hooks/useAuth.js';
import ProtectedRoute from './components/auth/ProtectedRoute.js';
import AppLayout from './components/layout/AppLayout.js';
import OrdersPage from './pages/order/OrderPage.js';
import AuthPage from './pages/auth/AuthPage.js';

function RootRedirect() {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
        <Spin size="large" />
      </div>
    );
  }

  if (isAuthenticated) {
    return <Navigate to="/order" replace />;
  }

  return <Navigate to="/auth" replace />;
}

const App = () => {
  return (
    <AuthProvider>
      <Routes>
        <Route path='/auth' element={<AuthPage />} />
        <Route path='/' element={<RootRedirect />} />
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
