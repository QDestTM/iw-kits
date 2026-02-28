import React from 'react';
import { Button, Space, Upload, Typography, message, Card } from 'antd';
import { UploadOutlined, PlusOutlined, ReloadOutlined, FilterOutlined } from '@ant-design/icons';
import { ordersApi } from '../../api/orders.api.js';

const { Title } = Typography;

interface OrdersToolbarProps {
    showFilters: boolean;
    onToggleFilters: () => void;
    onCreateClick: () => void;
    onRefresh: () => void;
}

export function OrdersToolbar({ showFilters, onToggleFilters, onCreateClick, onRefresh }: OrdersToolbarProps) {
    return (
        <Card style={{ marginBottom: '16px', borderRadius: '8px' }} styles={{ body: { padding: '16px 24px' } }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Title level={4} style={{ margin: 0 }}>Orders List</Title>
                <Space>
                    <Button
                        icon={<FilterOutlined />}
                        onClick={onToggleFilters}
                        type={showFilters ? 'primary' : 'default'}
                    >
                        Filters
                    </Button>

                    <Upload
                        name="file"
                        action={ordersApi.getImportUrl()}
                        showUploadList={false}
                        onChange={(info) => {
                            if (info.file.status === 'done') {
                                void message.success(`Import successful: ${info.file.response?.imported_total || 0} orders imported`);
                                onRefresh();
                            } else if (info.file.status === 'error') {
                                void message.error(`Import failed.`);
                            }
                        }}
                    >
                        <Button icon={<UploadOutlined />}>Import CSV</Button>
                    </Upload>

                    <Button type="primary" icon={<PlusOutlined />} onClick={onCreateClick}>
                        Create Manually
                    </Button>
                    <Button icon={<ReloadOutlined />} onClick={onRefresh} />
                </Space>
            </div>
        </Card>
    );
}
