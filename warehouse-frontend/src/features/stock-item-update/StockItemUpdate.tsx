import { Link, useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { useUpdateStockItemMutation } from './stock-item-update-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem2';
import IUpdateStockItem from '../../types/update-stock-item';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import ApplicationError from '../../types/application-error';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import Form from '../../components/form-elements/generic/Form';
import Submit from '../../components/form-elements/generic/Submit';

export function StockItemUpdate() {
  const stockItem = useSelector((state) => (state as RootState).selectedStockItem.current);
  const text = useAppSelector(selectText);

  const [updateStockItem, { status }] = useUpdateStockItemMutation();
  const [error, setError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

  const navigate = useNavigate();

  const [minimumQuantity, setMinimumQuantity] = useState(stockItem?.minimumQuantity || 0);
  const [minimumQuantityError, setMinimumQuantityError] = useState('');
  const [name, setName] = useState(stockItem?.name || '');
  const [nameError, setNameError] = useState('');
  const [quantity, setQuantity] = useState(stockItem?.quantity || 0);
  const [quantityError, setQuantityError] = useState('');

  const disabled =
    isInProgress
    || minimumQuantity < 0
    || minimumQuantityError !== ''
    || !name
    || nameError !== ''
    || quantity < 0
    || quantityError !== '';

  const handleSubmit = () => {
    if (disabled) {
      return;
    }

    const request: IUpdateStockItem = {
      minimumQuantity,
      name,
      quantity
    };

    if (!stockItem?.updateUrl) {
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
    <InProgressIndicator
      className='grid-large'
      isInProgress={status === QueryStatus.pending || isInProgress}
    >
      <Form
        className='container-grid stock-item'
        error={error}
        header={text.stockItemUpdateHeader}
        onSubmit={handleSubmit}
      >
        <StockItem
          isReadOnly={false}
          minimumQuantity={minimumQuantity}
          minimumQuantityError={minimumQuantityError}
          name={name}
          nameError={nameError}
          quantity={quantity}
          quantityError={quantityError}
          setMinimumQuantity={setMinimumQuantity}
          setMinimumQuantityError={setMinimumQuantityError}
          setName={setName}
          setNameError={setNameError}
          setQuantity={setQuantity}
          setQuantityError={setQuantityError}
          text={text}
        />
        <Submit
          disabled={disabled}
          id='StockItemUpdateSubmit'
          label={text.stockItemUpdateSubmitLabel}
        />
      </Form>
      <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
    </InProgressIndicator>
  )
}
