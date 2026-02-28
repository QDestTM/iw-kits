import React, { useState } from 'react';
import { Breadcrumb, Card } from 'antd';
import { HomeOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import { useOrders } from '../../hooks/useOrders.js';
import { OrdersToolbar } from '../../components/order/OrdersToolbar.js';
import { OrdersFilters } from '../../components/order/OrdersFilters.js';
import { OrdersTable } from '../../components/order/OrdersTable.js';
import CreateOrderModal from '../../components/order/CreateOrderModal.js';

export default function OrdersPage() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const orders = useOrders();

  return (
    <div style={{ background: '#f5f5f5', minHeight: '100vh', padding: '0 0 24px 0' }}>
      <div style={{ marginBottom: '16px' }}>
        <Breadcrumb items={[{ title: <Link to="/"><HomeOutlined /></Link> }, { title: 'Orders' }]} />
      </div>

      <OrdersToolbar
        showFilters={orders.showFilters}
        onToggleFilters={() => orders.setShowFilters(!orders.showFilters)}
        onCreateClick={() => setIsModalOpen(true)}
        onRefresh={() => void orders.fetchOrders()}
      />

      {orders.showFilters && (
        <OrdersFilters
          filters={orders.filters}
          onChange={orders.setFilters}
          onApply={orders.applyFilters}
          onReset={orders.resetFilters}
        />
      )}

      <Card style={{ borderRadius: '8px' }} styles={{ body: { padding: 0 } }}>
        <OrdersTable
          orders={orders.orders}
          loading={orders.loading}
          total={orders.totalCount}
          currentPage={orders.currentPage}
          pageSize={orders.pageSize}
          onTableChange={orders.handleTableChange}
        />
      </Card>

      <CreateOrderModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={() => void orders.fetchOrders()}
      />
    </div>
  );
}
