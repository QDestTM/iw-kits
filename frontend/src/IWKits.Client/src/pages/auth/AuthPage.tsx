import { useState } from 'react';
import { Card, Form, Input, Button, Typography, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth.js';

const { Title } = Typography;

export default function AuthPage() {
    const auth = useAuth();
    const [loading, setLoading] = useState(false);

    if (auth.isAuthenticated) {
        return <Navigate to="/order" replace />;
    }

    const handleLogin = async (values: { username: string; password: string }) => {
        setLoading(true);
        try {
            const error = await auth.login(values.username, values.password);
            if (error) {
                void message.error(error);
            }
        } catch {
            void message.error('Connection error');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{
            minHeight: '100vh',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        }}>
            <Card style={{ width: 400, borderRadius: 12, boxShadow: '0 8px 32px rgba(0,0,0,0.2)' }}>
                <Title level={3} style={{ textAlign: 'center', marginBottom: 24 }}>IW Kits</Title>
                <Form onFinish={(values) => void handleLogin(values)} layout="vertical" size="large">
                    <Form.Item name="username" rules={[{ required: true, message: 'Enter username' }]}>
                        <Input prefix={<UserOutlined />} placeholder="Username" />
                    </Form.Item>
                    <Form.Item name="password" rules={[{ required: true, message: 'Enter password' }]}>
                        <Input.Password prefix={<LockOutlined />} placeholder="Password" />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" htmlType="submit" loading={loading} block>
                            Login
                        </Button>
                    </Form.Item>
                </Form>
            </Card>
        </div>
    );
}
