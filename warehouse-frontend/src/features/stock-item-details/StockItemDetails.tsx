import { Navigate, useSearchParams } from 'react-router-dom';
import IText from '../../text/text'
import { useEffect, useState } from 'react';
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemQuery } from './stock-item-details-api-slice';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import StockItem from '../../components/form-elements/stock-item/StockItem';

export function StockItemDetails({
  text
}: {
  text: IText
}) {
  const [searchParams, setSearchParams] = useSearchParams();
  const [url] = useState(searchParams.get('url') || '');
  const { data: stockItem, status } = useGetStockItemQuery(url);

  useEffect(() => { setSearchParams(); }, [setSearchParams]);

  if (!url) {
    return (<Navigate to={AppRoutes.STOCK_ITEM_LIST} />)
  }

  if (status === QueryStatus.pending) {
    return (
      <h1>Loading</h1>
    );
  }

  if (status === QueryStatus.rejected) {
    return (
      <h1>no item found</h1>
    )
  }

  return (
    <StockItem
      headlineText={text.stockItemDetailsHeader}
      text={text}
      minimumQuantity={stockItem?.minimumQuantity}
      name={stockItem?.name}
      quantity={stockItem?.quantity}
    />
  )
}
