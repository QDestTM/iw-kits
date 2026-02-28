import { useState, useEffect, useCallback } from 'react';
import { message } from 'antd';
import { type Order, type OrdersQuery, ordersApi } from '../api/orders.api.js';
import dayjs from 'dayjs';

export interface OrdersFiltersState {
    dateRange: [dayjs.Dayjs | null, dayjs.Dayjs | null] | null;
    minAmount: number | null;
    maxAmount: number | null;
}

export interface UseOrdersReturn {
    orders: Order[];
    loading: boolean;
    totalCount: number;

    currentPage: number;
    pageSize: number;

    sortBy: string | undefined;
    descending: boolean | undefined;

    filters: OrdersFiltersState;
    showFilters: boolean;

    fetchOrders: () => Promise<void>;
    setShowFilters: (show: boolean) => void;
    setFilters: (filters: OrdersFiltersState) => void;
    applyFilters: () => void;
    resetFilters: () => void;
    handleTableChange: (page: number, size: number, sortBy?: string, descending?: boolean) => void;
}

export function useOrders(): UseOrdersReturn {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(false);
    const [showFilters, setShowFilters] = useState(false);

    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(24);
    const [totalCount, setTotalCount] = useState(0);

    const [sortBy, setSortBy] = useState<string | undefined>('timestamp');
    const [descending, setDescending] = useState<boolean | undefined>(true);

    const [filters, setFilters] = useState<OrdersFiltersState>({
        dateRange: null,
        minAmount: null,
        maxAmount: null,
    });

    const fetchOrders = useCallback(async () => {
        setLoading(true);
        try {
            const query: OrdersQuery = {
                page: currentPage,
                page_size: pageSize,
            };

            if (sortBy) query.sort_by = sortBy;
            if (descending !== undefined) query.descending = descending;

            if (filters.dateRange?.[0]) query.from_date = filters.dateRange[0].toISOString();
            if (filters.dateRange?.[1]) query.to_date = filters.dateRange[1].toISOString();
            if (filters.minAmount !== null) query.min_total_amount = filters.minAmount;
            if (filters.maxAmount !== null) query.max_total_amount = filters.maxAmount;

            const data = await ordersApi.getAll(query);
            setOrders(data.items);
            setTotalCount(data.total_count);
        } catch (error) {
            console.error("GET Error:", error);
            void message.error("Failed to load orders. Is the backend running?");
        } finally {
            setLoading(false);
        }
    }, [currentPage, pageSize, sortBy, descending, filters]);

    useEffect(() => {
        void fetchOrders();
    }, [fetchOrders]);

    const handleTableChange = (page: number, size: number, newSortBy?: string, newDescending?: boolean) => {
        setCurrentPage(page);
        setPageSize(size);
        if (newSortBy !== undefined) setSortBy(newSortBy);
        if (newDescending !== undefined) setDescending(newDescending);
    };

    const applyFilters = () => {
        setCurrentPage(1);
    };

    const resetFilters = () => {
        setFilters({ dateRange: null, minAmount: null, maxAmount: null });
        setCurrentPage(1);
    };

    return {
        orders, loading, totalCount,
        currentPage, pageSize,
        sortBy, descending,
        filters, showFilters,
        fetchOrders, setShowFilters, setFilters,
        applyFilters, resetFilters, handleTableChange,
    };
}
