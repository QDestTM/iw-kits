import React from 'react';
import { Modal, Form, InputNumber, message } from 'antd';
import { ordersApi } from '../../api/orders.api.js';

interface CreateOrderModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export default function CreateOrderModal({ isOpen, onClose, onSuccess }: CreateOrderModalProps) {
  const [form] = Form.useForm();

  const handleManualCreate = async (values: { lat: number, lon: number, subtotal: number }) => {
    try {
      await ordersApi.create({
        latitude: values.lat,
        longitude: values.lon,
        subtotal: values.subtotal
      });

      message.success('Order successfully created!');
      form.resetFields();
      onClose();
      onSuccess();
    } catch (error) {
      console.error("POST Error:", error);
      const errorMessage = error instanceof Error ? error.message : 'Failed to create order.';
      message.error(errorMessage);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      title="Create Order"
      open={isOpen}
      onCancel={handleCancel}
      onOk={() => form.submit()}
      okText="Create & Calculate"
      cancelText="Cancel"
    >
      <Form form={form} layout="vertical" onFinish={handleManualCreate}>
        <Form.Item label="Latitude" name="lat" rules={[{ required: true, message: 'Please enter latitude' }]}>
          <InputNumber style={{ width: '100%' }} placeholder="e.g., 40.7128" />
        </Form.Item>
        <Form.Item label="Longitude" name="lon" rules={[{ required: true, message: 'Please enter longitude' }]}>
          <InputNumber style={{ width: '100%' }} placeholder="e.g., -74.0060" />
        </Form.Item>
        <Form.Item label="Subtotal ($)" name="subtotal" rules={[{ required: true, message: 'Please enter subtotal' }]}>
          <InputNumber style={{ width: '100%' }} min={0} placeholder="Kit value" />
        </Form.Item>
      </Form>
    </Modal>
  );
}
