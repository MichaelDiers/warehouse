import IText from '../../text/text'
import StockItem from '../../components/form-elements/stock-item/StockItem';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { useDeleteStockItenMutation } from './stock-item-details-api-slice';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';

export function StockItemDetails({
  text
}: {
  text: IText
}) {
  const [globalError, setGlobalError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);
  const [deleteStockItem] = useDeleteStockItenMutation();
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);  

  const navigate = useNavigate();
  
  const handleDelete = () => {
    if (!stockItem || !stockItem.deleteUrl) {
      return;
    }

    setIsInProgress(true);
    deleteStockItem(stockItem.deleteUrl)
    .then(() => {
      navigate(AppRoutes.STOCK_ITEM_LIST);
    }).catch((err) => {
      setGlobalError(JSON.stringify(err));
    }).finally(() => {
      setIsInProgress(false);
    })
  }

  return (
    <StockItem
      globalError={globalError}
      deleteStockItem={handleDelete}
      headlineText={text.stockItemDetailsHeader}
      isInProgress={isInProgress}
      stockItem={stockItem}
      text={text}
      type='details'
    />
  )
}
