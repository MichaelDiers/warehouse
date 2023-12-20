import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { useCreateStockItemMutation } from './stock-item-create-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import ICreateStockItem from '../../types/create-stock-item';
import { useState } from 'react';
import ApplicationError from '../../types/application-error';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';

export function StockItemCreate() {
  const [createStockItem, { status }] = useCreateStockItemMutation();
  const [error, setError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

  const text = useAppSelector(selectText);

  const navigate = useNavigate();

  const handleSubmit = (stockItem: ICreateStockItem, isSubmitAndNew: boolean) => {
    setIsInProgress(true);
    createStockItem(stockItem)
      .unwrap()
      .then(() => {
        if (!isSubmitAndNew) {
          navigate(AppRoutes.STOCK_ITEM_LIST);
        }
      }).catch((err) => {
        if (err.name === ApplicationError.name) {
          setError(err.message);
        } else {
          const { status } = err;
          if (status) {
            switch (status) {
              case 400:
                setError(text.stockItemCreate400);
                break;
              case 401:
                setError(text.stockItemCreate401);
                break;
              case 403:
                setError(text.stockItemCreate403);
                break;
              case 409:
                setError(text.stockItemCreate409);
                break;
              default:
                setError(text.stockItemCreate500_1);
                break;
            }
          } else {
            setError(text.stockItemCreate500_2);
          }
        }
      }).finally(() => {
        setIsInProgress(false);
      });
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending || isInProgress}>
      <StockItem
        create={handleSubmit}
        globalError={error}
        headlineText={text.stockItemCreateHeader}
        text={text}
        type={'create'}
      />
    </InProgressIndicator>
  )
}
