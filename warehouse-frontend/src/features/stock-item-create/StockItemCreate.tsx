import { useNavigate } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useCreateStockItemMutation } from './stock-item-create-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem';
import ICreateStockItem from '../../types/create-stock-item';
import { useState } from 'react';

export function StockItemCreate({
  text
}: {
  text: IText
}) {
  const [createStockItem] = useCreateStockItemMutation();
  const [globalError, setGlobalError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

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
        setGlobalError(JSON.stringify(err));
      }).finally(() => {
        setIsInProgress(false);
      });
  }

  return (
    <>
      <StockItem
        create={handleSubmit}
        globalError={globalError}
        headlineText={text.stockItemCreateHeader}
        isInProgress={isInProgress}
        text={text}
        type={'create'}
      />
    </>
  )
}
