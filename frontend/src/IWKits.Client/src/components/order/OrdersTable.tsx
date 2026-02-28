import React from 'react';
import { Table, Space, Tag, Tooltip } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table/index.js';
import type { SorterResult } from 'antd/es/table/interface.js';
import { type Order } from '../../api/orders.api.js';

const columns: ColumnsType<Order> = [
  { title: 'ID', dataIndex: 'id', key: 'id', width: 80, render: (id: string) => <span title={id}>{id?.substring(0, 8)}...</span> },
  {
    title: 'Date & Time',
    dataIndex: 'timestamp',
    key: 'timestamp',
    width: 180,
    sorter: true,
    render: (ts) => ts ? new Date(ts).toLocaleString() : '-'
  },
  {
    title: 'Location',
    key: 'coordinates',
    width: 160,
    render: (_, record) => `${record.latitude?.toFixed(4)}, ${record.longitude?.toFixed(4)}`
  },
  { title: 'Subtotal', dataIndex: 'subtotal', key: 'subtotal', width: 100, sorter: true, render: (val) => `$${val?.toFixed(2) || 0}` },
  {
    title: 'Tax',
    key: 'tax_amount',
    dataIndex: 'tax_amount',
    width: 110,
    sorter: true,
    render: (_, record) => (
      <Tooltip title={`Rate: ${((record.composite_tax_rate || 0) * 100).toFixed(3)}%`}>
        <span>${record.tax_amount?.toFixed(2) || 0} <InfoCircleOutlined style={{ color: '#1890ff', fontSize: '12px' }} /></span>
      </Tooltip>
    )
  },
  {
    title: 'Total',
    dataIndex: 'total_amount',
    key: 'total_amount',
    width: 100,
    sorter: true,
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
        {record.breakdown?.county_rate > 0 && <Tag color="purple">County: {record.breakdown.county_rate}</Tag>}
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

const sortFieldMap: Record<string, string> = {
  timestamp: 'timestamp',
  subtotal: 'subtotal',
  tax_amount: 'tax_amount',
  total_amount: 'total_amount',
};

interface OrdersTableProps {
  orders: Order[];
  loading: boolean;
  total: number;
  currentPage: number;
  pageSize: number;
  onTableChange: (page: number, pageSize: number, sortBy?: string, descending?: boolean) => void;
}

export function OrdersTable({ orders, loading, total, currentPage, pageSize, onTableChange }: OrdersTableProps) {
  const handleChange = (
    pagination: TablePaginationConfig,
    _filters: Record<string, any>,
    sorter: SorterResult<Order> | SorterResult<Order>[]
  ) => {
    const newPage = pagination.current || 1;
    const newPageSize = pagination.pageSize || pageSize;

    const singleSorter = Array.isArray(sorter) ? sorter[0] : sorter;
    const columnKey = singleSorter?.columnKey as string | undefined;
    const sortField = columnKey ? sortFieldMap[columnKey] : undefined;
    const descending = singleSorter?.order === 'descend' ? true
      : singleSorter?.order === 'ascend' ? false
        : undefined;

    onTableChange(newPage, newPageSize, sortField, descending);
  };

  return (
    <Table
      columns={columns}
      dataSource={orders}
      rowKey="id"
      loading={loading}
      onChange={handleChange}
      pagination={{
        current: currentPage,
        pageSize: pageSize,
        total: total,
        showSizeChanger: true,
        pageSizeOptions: ['10', '25', '50', '100'],
        showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} orders`,
      }}
      scroll={{ x: 'max-content' }}
      locale={{ emptyText: 'No orders found.' }}
    />
  );
}
