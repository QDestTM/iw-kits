import { Routes, Route, Navigate } from 'react-router-dom';
import AppLayout from "./components/layout/AppLayout.js";
import OrdersPage from "./pages/order/OrderPage.js";


const App = () => {
  return (
    <Routes>
      <Route path='/' element={<Navigate to="/order" replace />} />
      <Route
        path="/*"
        element={
            <AppLayout>
              <Routes>
                <Route path='/order' element={<OrdersPage />} />
              </Routes>
            </AppLayout>
        }/>
    </Routes>
  );
};

export default App;
