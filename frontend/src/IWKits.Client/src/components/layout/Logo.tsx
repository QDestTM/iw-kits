import { Flex } from 'antd';
import { RocketOutlined } from '@ant-design/icons';

interface LogoProps {
  collapsed: boolean;
}

export const Logo = ({ collapsed }: LogoProps) => {  return (
    <Flex
      align="center"
      justify="center"
      gap={8}
      style={{ height: 32, margin: 16 }}
    >
      <RocketOutlined style={{ fontSize: '24px', color: '#1677ff' }} />

      {!collapsed && (
        <span style={{ fontWeight: 'bold', fontSize: 16, color: '#fff' }}>
          Wellness Kits
        </span>
      )}
    </Flex>
  );
};
