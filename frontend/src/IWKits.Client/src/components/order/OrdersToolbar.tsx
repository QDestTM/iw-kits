import React, { useState } from 'react';
import { Button, Space, Upload, Typography, message, Card } from 'antd';
import { UploadOutlined, PlusOutlined, ReloadOutlined, FilterOutlined, LoadingOutlined } from '@ant-design/icons';
import { ordersApi } from '../../api/orders.api.js';

const { Title } = Typography;

interface OrdersToolbarProps {
    showFilters: boolean;
    onToggleFilters: () => void;
    onCreateClick: () => void;
    onRefresh: () => void;
}

export function OrdersToolbar({ showFilters, onToggleFilters, onCreateClick, onRefresh }: OrdersToolbarProps) {
    const [isUploading, setIsUploading] = useState(false);
    const [loadingMessage, setLoadingMessage] = useState<string | null>(null);

    const handleUploadChange = (info: any) => {
        if (info.file.status === 'uploading') {
            setIsUploading(true);
            setLoadingMessage('Завантаження та обробка даних...');
            void message.loading({ content: 'Завантаження та обробка CSV файлу. Будь ласка, зачекайте...', key: 'upload', duration: 0 });
        } else if (info.file.status === 'done') {
            setIsUploading(false);
            setLoadingMessage(null);
            message.destroy('upload');
            void message.success(`Імпорт успішний: ${info.file.response?.imported_total || 0} замовлень імпортовано`);
            onRefresh();
        } else if (info.file.status === 'error') {
            setIsUploading(false);
            setLoadingMessage(null);
            message.destroy('upload');
            void message.error('Помилка імпорту. Перевірте файл та спробуйте ще раз.');
        }
    };

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
                        headers={{
                            Authorization: `Bearer ${localStorage.getItem('access_token') ?? ''}`,
                        }}
                        onChange={handleUploadChange}
                        disabled={isUploading}
                    >
                        <Button 
                            icon={isUploading ? <LoadingOutlined /> : <UploadOutlined />} 
                            loading={isUploading}
                            disabled={isUploading}
                        >
                            {isUploading ? 'Обробка...' : 'Import CSV'}
                        </Button>
                    </Upload>

                    <Button type="primary" icon={<PlusOutlined />} onClick={onCreateClick} disabled={isUploading}>
                        Create Manually
                    </Button>
                    <Button icon={<ReloadOutlined />} onClick={onRefresh} disabled={isUploading} />
                </Space>
            </div>
            {loadingMessage && (
                <div style={{ marginTop: '8px', color: '#1890ff', fontSize: '14px' }}>
                    {loadingMessage}
                </div>
            )}
        </Card>
    );
}
