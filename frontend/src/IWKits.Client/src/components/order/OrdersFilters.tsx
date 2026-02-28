import React from 'react';
import { Button, Space, Card, DatePicker, InputNumber, Row, Col } from 'antd';
import { ClearOutlined } from '@ant-design/icons';
import type { OrdersFiltersState } from '../../hooks/useOrders.js';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

interface OrdersFiltersProps {
    filters: OrdersFiltersState;
    onChange: (filters: OrdersFiltersState) => void;
    onApply: () => void;
    onReset: () => void;
}

export function OrdersFilters({ filters, onChange, onApply, onReset }: OrdersFiltersProps) {
    return (
        <Card style={{ marginBottom: '16px', borderRadius: '8px' }} styles={{ body: { padding: '16px 24px' } }}>
            <Row gutter={[16, 12]} align="bottom">
                <Col>
                    <div style={{ marginBottom: 4, fontSize: 13, color: '#666' }}>Date Range</div>
                    <RangePicker
                        value={filters.dateRange as any}
                        onChange={(dates) => onChange({
                            ...filters,
                            dateRange: dates as [dayjs.Dayjs | null, dayjs.Dayjs | null] | null,
                        })}
                        showTime
                        format="YYYY-MM-DD HH:mm"
                    />
                </Col>
                <Col>
                    <div style={{ marginBottom: 4, fontSize: 13, color: '#666' }}>Min Total ($)</div>
                    <InputNumber<number>
                        value={filters.minAmount}
                        onChange={(val) => onChange({ ...filters, minAmount: val })}
                        min={0}
                        placeholder="0.00"
                        style={{ width: 120 }}
                    />
                </Col>
                <Col>
                    <div style={{ marginBottom: 4, fontSize: 13, color: '#666' }}>Max Total ($)</div>
                    <InputNumber<number>
                        value={filters.maxAmount}
                        onChange={(val) => onChange({ ...filters, maxAmount: val })}
                        min={0}
                        placeholder="999.99"
                        style={{ width: 120 }}
                    />
                </Col>
                <Col>
                    <Space>
                        <Button type="primary" onClick={onApply}>Apply</Button>
                        <Button icon={<ClearOutlined />} onClick={onReset}>Reset</Button>
                    </Space>
                </Col>
            </Row>
        </Card>
    );
}
