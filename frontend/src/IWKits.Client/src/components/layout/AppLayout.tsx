import { Layout } from 'antd';
import {Sidebar} from "./Sidebar.js";
import type {ReactNode} from "react";

type Props = {
  children: ReactNode;
};

const AppLayout = ({ children }: Props) => (
  <Layout style={{ minHeight: '100vh' }}>
    <Sidebar />
    <Layout>
      <Layout.Content style={{ margin: '24px' }}>
        {children}
      </Layout.Content>
    </Layout>
  </Layout>
);
export default AppLayout;
