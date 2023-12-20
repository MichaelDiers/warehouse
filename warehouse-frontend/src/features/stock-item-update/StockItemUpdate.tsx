import { useNavigate } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useUpdateStockItemMutation } from './stock-item-update-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import IUpdateStockItem from '../../types/update-stock-item';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import ApplicationError from '../../types/application-error';

export function StockItemUpdate({
  text
}: {
  text: IText
}) {
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);

  const [updateStockItem, { status }] = useUpdateStockItemMutation();
  const [error, setError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = (request: IUpdateStockItem) => {
    if (!stockItem || !stockItem.updateUrl) {
      setError(text.stockItemUpdate500_1);
      return;
    }

    setIsInProgress(true);
    updateStockItem({ request, url: stockItem.updateUrl })
      .unwrap()
      .then(() => {
        navigate(AppRoutes.STOCK_ITEM_LIST);
      }).catch((err) => {
        if (err.name === ApplicationError.name) {
          setError(err.message);
        } else {
          const { status } = err;
          if (status) {
            switch (status) {
              case 400:
                setError(text.stockItemUpdate400);
                break;
              case 401:
                setError(text.stockItemUpdate401);
                break;
              case 403:
                setError(text.stockItemUpdate403);
                break;
              case 404:
                setError(text.stockItemUpdate404);
                break;
              case 409:
                setError(text.stockItemUpdate409);
                break;
              default:
                setError(text.stockItemUpdate500_2);
                break;
            }
          } else {
            setError(text.stockItemUpdate500_3);
          }
        }
      }).finally(() => {
        setIsInProgress(false);
      });
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <StockItem
        globalError={error}
        headlineText={text.stockItemUpdateHeader}
        isInProgress={isInProgress}
        stockItem={stockItem}
        text={text}
        type='update'
        update={handleSubmit}
      />
    </InProgressIndicator>
  )
}
