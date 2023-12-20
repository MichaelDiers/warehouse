import { useNavigate } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useUpdateStockItemMutation } from './stock-item-update-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import IUpdateStockItem from '../../types/update-stock-item';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';

export function StockItemUpdate({
  text
}: {
  text: IText
}) {
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);

  const [updateStockItem] = useUpdateStockItemMutation();
  const [globalError, setGlobalError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = (request: IUpdateStockItem) => {
    if (!stockItem || !stockItem.updateUrl) {
      setGlobalError('Cannot update stock item');
      return;
    }

    setIsInProgress(true);
    updateStockItem({ request, url: stockItem.updateUrl })
      .unwrap()
      .then(() => {
        navigate(AppRoutes.STOCK_ITEM_LIST);
      }).catch((err) => {
        setGlobalError(JSON.stringify(err));
      }).finally(() => {
        setIsInProgress(false);
      });
  }

  return (
    <StockItem
      globalError={globalError}
      headlineText={text.stockItemUpdateHeader}
      isInProgress={isInProgress}
      stockItem={stockItem}
      text={text}
      type='update'
      update={handleSubmit}
    />
  )
}
