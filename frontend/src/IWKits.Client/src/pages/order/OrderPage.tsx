import React, { useState, useEffect } from 'react';
import { Button, Space, Upload, Typography, message, Breadcrumb, Card } from 'antd';
import { UploadOutlined, PlusOutlined, HomeOutlined, ReloadOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import { type Order, ordersApi } from "../../api/orders.api.js";
import { OrdersTable } from "../../components/order/OrdersTable.js";
import CreateOrderModal from "../../components/order/CreateOrderModal.js";

const { Title } = Typography;

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const data = await ordersApi.getAll();
      setOrders(data);
    } catch (error) {
      console.error("GET Error:", error);
      void message.error("Failed to load orders. Is the backend running?");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    void fetchOrders();
  }, []);

  return (
    <div style={{ background: '#f5f5f5', minHeight: '100vh', padding: '0 0 24px 0' }}>
      <div style={{ marginBottom: '16px' }}>
        <Breadcrumb items={[{ title: <Link to="/"><HomeOutlined /></Link> }, { title: 'Orders' }]} />
      </div>

      <Card style={{ marginBottom: '16px', borderRadius: '8px' }} styles={{ body: { padding: '16px 24px' } }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Title level={4} style={{ margin: 0 }}>Orders List</Title>
          <Space>
            <Upload
              name="file"
              action={ordersApi.getImportUrl()}
              showUploadList={false}
              onChange={(info) => {
                if (info.file.status === 'done') {
                  void message.success(`Import successful: ${info.file.response?.imported_total || 0} orders imported`);
                  void fetchOrders();
                } else if (info.file.status === 'error') {
                  void message.error(`Import failed.`);
                }
              }}
            >
              <Button icon={<UploadOutlined />}>Import CSV</Button>
            </Upload>

            <Button type="primary" icon={<PlusOutlined />} onClick={() => setIsModalOpen(true)}>
              Create Manually
            </Button>
            <Button icon={<ReloadOutlined />} onClick={() => void fetchOrders()} />
          </Space>
        </div>
      </Card>

      <Card style={{ borderRadius: '8px' }} styles={{ body: { padding: 0 } }}>
        <OrdersTable orders={orders} loading={loading} />
      </Card>

      <CreateOrderModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={() => void fetchOrders()}
      />
    </div>
  );
}
