import StockItem from '../../components/form-elements/stock-item/StockItem';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { useDeleteStockItenMutation } from './stock-item-details-api-slice';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import ApplicationError from '../../types/application-error';

export function StockItemDetails() {
  const [isInProgress, setIsInProgress] = useState(false);
  const [error, setError] = useState('');

  const [deleteStockItem, { status }] = useDeleteStockItenMutation();
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);
  const text = useAppSelector(selectText);

  const navigate = useNavigate();

  const handleDelete = () => {
    if (!stockItem || !stockItem.deleteUrl) {
      setError(text.stockItemDelete500_1);
      return;
    }

    setIsInProgress(true);
    deleteStockItem(stockItem.deleteUrl)
      .then(() => {
        navigate(AppRoutes.STOCK_ITEM_LIST);
      }).catch((err) => {
        if (err.name === ApplicationError.name) {
          setError(err.message);
        } else {
          const { status } = err;
          if (status) {
            switch (status) {
              case 401:
                setError(text.stockItemDelete401);
                break;
              case 403:
                setError(text.stockItemDelete403);
                break;
              case 404:
                setError(text.stockItemDelete404);
                break;
              default:
                setError(text.stockItemDelete500_2);
                break;
            }
          } else {
            setError(text.stockItemDelete500_3);
          }
        }
      }).finally(() => {
        setIsInProgress(false);
      })
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <StockItem
        deleteStockItem={handleDelete}
        globalError={error}
        headlineText={text.stockItemDetailsHeader}
        isInProgress={isInProgress}
        stockItem={stockItem}
        text={text}
        type='details'
      />
    </InProgressIndicator>
  )
}
