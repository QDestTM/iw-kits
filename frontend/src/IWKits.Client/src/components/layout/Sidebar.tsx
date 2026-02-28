import { useState } from 'react';
import { ShoppingCartOutlined, LogoutOutlined, UserOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Layout, Menu, Typography } from 'antd';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { Logo } from './Logo.js';
import { useAuth } from '../../hooks/useAuth.js';

const { Sider } = Layout;
const { Text } = Typography;

type MenuItem = Required<MenuProps>['items'][number];

const getItems = (onLogout: () => void): MenuItem[] => [
  {
    key: '/order',
    label: <Link to="/order">Orders</Link>,
    icon: <ShoppingCartOutlined />,
  },
  {
    key: '/logout',
    label: 'Logout',
    onClick: onLogout,
    icon: <LogoutOutlined />,
    danger: true,
  },
];

export const Sidebar = () => {
  const [collapsed, setCollapsed] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/auth');
  };

  return (
    <Sider
      collapsible
      collapsed={collapsed}
      onCollapse={setCollapsed}
      style={{
        overflow: 'auto',
        height: '100vh',
        position: 'sticky',
        insetInlineStart: 0,
        top: 0,
      }}
    >
      <Logo collapsed={collapsed} />

      {!collapsed && user && (
        <div style={{ padding: '0 16px 12px', textAlign: 'center' }}>
          <UserOutlined style={{ color: '#fff', marginRight: 6 }} />
          <Text style={{ color: '#ffffffa6', fontSize: 13 }}>{user.username}</Text>
        </div>
      )}

      <Menu
        theme="dark"
        selectedKeys={[location.pathname]}
        mode="inline"
        items={getItems(handleLogout)}
      />
    </Sider>
  );
};
