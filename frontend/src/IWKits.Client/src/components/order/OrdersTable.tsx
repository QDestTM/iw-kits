import React from 'react';
import { Table, Space, Tag, Tooltip } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table/index.js';
import { type Order } from '../../api/orders.api.js';

const columns: ColumnsType<Order> = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80, render: (id: string) => <span title={id}>{id?.substring(0, 8)}...</span> },
  {
    title: 'Date & Time',
    dataIndex: 'timestamp',
    key: 'timestamp',
    width: 170,
    render: (ts) => ts ? new Date(ts).toLocaleString() : '-'
  },
  {
    title: 'Location',
    key: 'coordinates',
    width: 160,
    render: (_, record) => `${record.latitude?.toFixed(4)}, ${record.longitude?.toFixed(4)}`
  },
  { title: 'Subtotal', dataIndex: 'subtotal', key: 'subtotal', width: 100, render: (val) => `$${val?.toFixed(2) || 0}` },
  {
    title: 'Tax',
    key: 'tax',
    width: 110,
    render: (_, record) => (
      <Tooltip title={`Rate: ${((record.composite_tax_rate || 0) * 100).toFixed(3)}%`}>
        <span>${record.tax_amount?.toFixed(2) || 0} <InfoCircleOutlined style={{color: '#1890ff', fontSize: '12px'}}/></span>
      </Tooltip>
    )
  },
  {
    title: 'Total',
    dataIndex: 'total_amount',
    key: 'total',
    width: 100,
    render: (val) => <strong style={{ color: '#1677ff' }}>${val?.toFixed(2) || 0}</strong>
  },
  {
    title: 'Breakdown',
    key: 'breakdown',
    width: 320,
    render: (_, record) => (
      <Space orientation="horizontal" size={[0, 4]} wrap>
        {record.breakdown?.state_rate > 0 && <Tag color="blue">State: {record.breakdown.state_rate}</Tag>}
        {record.breakdown?.city_rate > 0 && <Tag color="cyan">City: {record.breakdown.city_rate}</Tag>}
        {record.breakdown?.country_rate > 0 && <Tag color="purple">County: {record.breakdown.country_rate}</Tag>}
        {record.breakdown?.special_rate > 0 && <Tag color="orange">Special: {record.breakdown.special_rate}</Tag>}
      </Space>
    )
  },
  {
    title: 'Jurisdictions',
    key: 'jurisdictions',
    width: 320,
    render: (_, record) => (
      <Space orientation="horizontal" size={[0, 4]} wrap>
        {record.jurisdictions?.map((j, idx) => (
          <Tooltip key={idx} title={`Rate: ${j.rate}`}>
            <Tag variant="filled">{j.name}</Tag>
          </Tooltip>
        ))}
      </Space>
    )
  },
];

interface OrdersTableProps {
  orders: Order[];
  loading: boolean;
}

export function OrdersTable({ orders, loading }: OrdersTableProps) {
  return (
    <Table
      columns={columns}
      dataSource={orders}
      rowKey="id"
      loading={loading}
      pagination={{ pageSize: 10 }}
      scroll={{ x: 'max-content' }}
      locale={{ emptyText: 'No orders found.' }}
    />
  );
}
