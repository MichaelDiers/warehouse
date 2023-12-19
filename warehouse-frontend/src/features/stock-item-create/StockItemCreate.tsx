import { Link, redirect, useNavigate } from 'react-router-dom';
import IText from "../../text/text";
import AppRoutes from '../../types/app-routes.enum';
import { useState } from 'react';
import StockItemName from '../../components/form-elements/StockItemName';
import Quantity from '../../components/form-elements/Quantity';
import MinimumQuantity from '../../components/form-elements/MinimumQuantity';
import Submit from '../../components/form-elements/Submit';
import Form from '../../components/form-elements/Form';
import { ICreateStockItemRequest, useCreateStockItemMutation } from './stock-item-create-api-slice';

export function StockItemCreate({
  text
}: {
  text: IText
}) {
  
  const [createStockItem, { status }] = useCreateStockItemMutation();

  const [error, setError] = useState('');
  const [name, setName] = useState('');
  const [nameError, setNameError] = useState('');
  const [quantity, setQuantity] = useState('');
  const [quantityError, setQuantityError] = useState('');
  const [minimumQuantity, setMinimumQuantity] = useState('');
  const [minimumQuantityError, setMinimumQuantityError] = useState('');
  const [isSubmitAndNew, setIsSubmitAndNew] = useState(false);
  const navigate = useNavigate();

  const disabled: boolean = 
    !name
    || nameError !== ''
    || !quantity
    || quantityError !== ''
    || !minimumQuantity
    || minimumQuantityError !== '';

    const onSubmit = () => {
      setError('');

      const request : ICreateStockItemRequest = {
        minimumQuantity,
        name,
        quantity
      };

      createStockItem(request).unwrap().then((result) => {
        if (isSubmitAndNew) {
          setName('');
          setQuantity('');
          setMinimumQuantity('');
        } else {
          navigate(AppRoutes.STOCK_ITEM_LIST);
        }
        
      }).catch((err) => {
        setError(JSON.stringify(err));
      })
    }

  return (
    <>
      <h1>{text.stockItemCreateHeader}</h1>
      <Form onSubmit={onSubmit}>

        <StockItemName
          error={nameError}
          setError={setNameError}
          setValue={setName}
          text={text}
          value={name}
        />
        <Quantity
          error={quantityError}
          setError={setQuantityError}
          setValue={setQuantity}
          text={text}
          value={quantity}
        />
        <MinimumQuantity
          error={minimumQuantityError}
          setError={setMinimumQuantityError}
          setValue={setMinimumQuantity}
          text={text}
          value={minimumQuantity}
        />
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
      </Form>
      <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
    </>
  )
}
