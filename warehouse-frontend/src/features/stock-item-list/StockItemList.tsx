import { Link } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemsQuery } from './stock-item-list-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import { InProgressIndicator } from '../../components/InProgress';

export function StockItemList() {
  const { data: stockItems, status } = useGetStockItemsQuery();
  const text = useAppSelector(selectText);

  let stockItemList;
  if (status === QueryStatus.pending || status === QueryStatus.uninitialized) {
    stockItemList = (<div>{text.stockItemListPending}</div>);
  } else if (status === QueryStatus.rejected) {
    stockItemList = (<div>{text.stockItemListRejected}</div>);
  } else {
    stockItemList = (
      <ul>
        {stockItems?.map((stockItem, index) => (
          <li key={index}>
            <StockItem
              stockItem={stockItem}
              text={text}
              type='list'
            />
          </li>
        ))}
      </ul>
    );
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <h1>{text.stockItemListHeader}</h1>
      {stockItemList}
      <Link to={AppRoutes.STOCK_ITEM_CREATE}>
        {text.stockItemCreateLinkLabel}
      </Link>
    </InProgressIndicator>
  )
}
