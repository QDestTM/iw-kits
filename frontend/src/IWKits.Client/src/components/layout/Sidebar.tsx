import { useState } from 'react';
import { ShoppingCartOutlined, LogoutOutlined } from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Layout, Menu } from 'antd';
import { Link, useLocation } from 'react-router-dom';
import {Logo} from "./Logo.js";

const { Sider } = Layout;

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
  },
];

export const Sidebar = () => {
  const [collapsed, setCollapsed] = useState(false);
  const location = useLocation();

  const handleLogout = () => {
    alert('Тут буде логіка виходу');
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

      <Menu
        theme="dark"
        selectedKeys={[location.pathname]}
        mode="inline"
        items={getItems(handleLogout)}
      />
    </Sider>
  );
};
