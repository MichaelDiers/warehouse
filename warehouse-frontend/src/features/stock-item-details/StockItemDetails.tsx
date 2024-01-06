import StockItem from '../../components/form-elements/stock-item/StockItem2';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { useDeleteStockItenMutation } from './stock-item-details-api-slice';
import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import ApplicationError from '../../types/application-error';
import Form from '../../components/form-elements/generic/Form';
import Submit from '../../components/form-elements/generic/Submit';

export function StockItemDetails() {
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
      });
  }

  return (
    <InProgressIndicator
      className='grid-large'
      isInProgress={status === QueryStatus.pending}>
      <Form
        className='container-grid stock-item'
        error={error}
        header={text.stockItemDetailsHeader}
        onSubmit={handleDelete}
      >
        <StockItem
          isReadOnly={true}
          minimumQuantity={stockItem?.minimumQuantity}
          name={stockItem?.name}
          quantity={stockItem?.quantity}
          text={text}
        />
        <div className='element-group-2 form-element'>
          <Submit id='deleteStockItemSubmit' label={text.stockItemDeleteDeleteSubmitLabel} />
          <Link to={AppRoutes.STOCK_ITEM_UPDATE}>{text.stockItemUpdateLinkLabel}</Link>
        </div>

      </Form>
      <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
    </InProgressIndicator>
  )
}
