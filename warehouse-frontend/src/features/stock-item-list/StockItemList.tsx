import { Link } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { useGetStockItemsQuery } from './stock-item-list-api-slice';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import { InProgressIndicator } from '../../components/InProgress';
import { useDispatch } from 'react-redux';
import { setSelectedStockItem } from '../../app/selected-stock-item-slice';
import IFrontendStockItem from '../../types/frontend-stock-item';

export function StockItemList() {
  const { data: stockItems, status } = useGetStockItemsQuery();
  const text = useAppSelector(selectText);

  const dispatch = useDispatch();
  const handleOnClick = (stockItem: IFrontendStockItem) => {
    dispatch(setSelectedStockItem(stockItem));
  }

  let stockItemList;
  if (status === QueryStatus.pending || status === QueryStatus.uninitialized) {
    stockItemList = (<div>{text.stockItemListPending}</div>);
  } else if (status === QueryStatus.rejected) {
    stockItemList = (<div>{text.stockItemListRejected}</div>);
  } else if (stockItems?.some(x => x) !== true) {
    stockItemList = (
      <div className='stock-item-list'>
        {text.emptyStockItemList}
      </div>
    );
  } else {
    stockItemList = (
      <div className='stock-item-list'>
        <div>
          <div>{text.stockItemNameLabel}</div>
          <div>{text.stockItemQuantityLabel}</div>
        </div>
        {stockItems?.map((stockItem, index) => (
          <Link
            key={index}
            onClick={() => handleOnClick(stockItem)}
            to={AppRoutes.STOCK_ITEM_DETAILS}
          >
            <div>{stockItem.name}</div>
            <div>{stockItem.quantity}</div>
          </Link>
        ))}
      </div>
    );
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <div className='grid-large'>
        <div className='stock-item-form'>
          <h2>{text.stockItemListHeader}</h2>
          {stockItemList}
        </div>
        <Link to={AppRoutes.STOCK_ITEM_CREATE}>
          {text.stockItemCreateLinkLabel}
        </Link>
      </div>
    </InProgressIndicator>
  )
}
