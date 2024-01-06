import { Link, useNavigate } from 'react-router-dom';
import AppRoutes from '../../types/app-routes.enum';
import { useCreateStockItemMutation } from './stock-item-create-api-slice';
import StockItem from '../../components/form-elements/stock-item/StockItem2';
import ICreateStockItem from '../../types/create-stock-item';
import { useState } from 'react';
import ApplicationError from '../../types/application-error';
import { useAppSelector } from '../../app/hooks';
import { selectText } from '../../app/selectors';
import { InProgressIndicator } from '../../components/InProgress';
import { QueryStatus } from '@reduxjs/toolkit/dist/query';
import Form from '../../components/form-elements/generic/Form';
import Submit from '../../components/form-elements/generic/Submit';

export function StockItemCreate() {
  const [createStockItem, { status }] = useCreateStockItemMutation();

  const [error, setError] = useState('');
  const [isInProgress, setIsInProgress] = useState(false);

  const [minimumQuantity, setMinimumQuantity] = useState(0);
  const [minimumQuantityError, setMinimumQuantityError] = useState('');
  const [name, setName] = useState('');
  const [nameError, setNameError] = useState('');
  const [quantity, setQuantity] = useState(0);
  const [quantityError, setQuantityError] = useState('');

  const [isSubmitAndNew, setIsSubmitAndNew] = useState(false);

  const text = useAppSelector(selectText);

  const navigate = useNavigate();

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

    const stockItem: ICreateStockItem = {
      minimumQuantity: minimumQuantity,
      name: name,
      quantity: quantity
    };

    setIsInProgress(true);

    createStockItem(stockItem)
      .unwrap()
      .then(() => {
        if (!isSubmitAndNew) {
          navigate(AppRoutes.STOCK_ITEM_LIST);
        } else {
          setName('');
          setQuantity(0);
          setMinimumQuantity(0);
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
    <InProgressIndicator
      className='grid-large'
      isInProgress={status === QueryStatus.pending || isInProgress}
    >
      <Form
        className='container-grid stock-item'
        error={error}
        header={text.stockItemCreateHeader}
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
        <div className='element-group-2 form-element'>
          <Submit
            disabled={disabled}
            id='StockItemCreateSubmit'
            label={text.stockItemCreateSubmitLabel}
            onClick={() => setIsSubmitAndNew(false)}
          />
          <Submit
            disabled={disabled}
            id='StockItemCreateAndNewSubmit'
            label={text.stockItemCreateSubmitAndNewLabel}
            onClick={() => setIsSubmitAndNew(true)}
          />
        </div>
      </Form>
      <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
    </InProgressIndicator>
  )
}
