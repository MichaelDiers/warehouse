import { Link } from 'react-router-dom';
import IText from '../../../text/text';
import MinimumQuantity from './MinimumQuantity';
import Quantity from './Quantity';
import StockItemName from './StockItemName';
import { useState } from 'react';
import Form from '../generic/Form';
import Submit from '../generic/Submit';
import ICreateStockItem from '../../../types/create-stock-item';
import AppRoutes from '../../../types/app-routes.enum';
import IUpdateStockItem from '../../../types/update-stock-item';
import IFrontendStockItem from '../../../types/frontend-stock-item';
import { useDispatch } from 'react-redux';
import { setSelectedStockItem } from '../../../app/selected-stock-item-slice';

const StockItem = ({
  create,
  globalError,
  headlineText,
  isInProgress,
  stockItem,
  text,
  type = 'details',
  update,
}: {
  create?: (stockItem: ICreateStockItem, isSubmitAndNew: boolean) => void,
  globalError?: string,
  headlineText?: string,
  isInProgress?: boolean,
  stockItem?: IFrontendStockItem,
  text: IText,
  type: 'create' | 'details' | 'list' | 'update',
  update?: (stockItem: IUpdateStockItem) => void,
}) => {
  const dispatch = useDispatch();

  const [localMinimumQuantity, setLocalMinimumQuantity] = useState(stockItem?.minimumQuantity || 0);
  const [localMinimumQuantityError, setLocalMinimumQuantityError] = useState('');
  const [localName, setLocalName] = useState(stockItem?.name || '');
  const [localNameError, setLocalNameError] = useState('');
  const [localQuantity, setLocalQuantity] = useState(stockItem?.quantity || 0);
  const [localQuantityError, setLocalQuantityError] = useState('');

  const [isSubmitAndNew, setIsSubmitAndNew] = useState<boolean>(false);
  
  const isReadOnly = type === 'details' || type === 'list';

  const headlineElement = (headlineText ? <h1>{headlineText}</h1> : <></>);

  const stockItemElement = (
    <>
      <StockItemName
        error={isReadOnly ? undefined : localNameError}
        setError={isReadOnly ? undefined : setLocalNameError}
        setValue={isReadOnly ? undefined : setLocalName}
        text={text}
        value={localName}
      />
      <Quantity
        error={isReadOnly ? undefined : localQuantityError}
        setError={isReadOnly ? undefined : setLocalQuantityError}
        setValue={isReadOnly ? undefined : setLocalQuantity}
        text={text}
        value={localQuantity}
      />
      <MinimumQuantity
        error={isReadOnly ? undefined : localMinimumQuantityError}
        setError={isReadOnly ? undefined : setLocalMinimumQuantityError}
        setValue={isReadOnly ? undefined : setLocalMinimumQuantity}
        text={text}
        value={localMinimumQuantity}
      />
    </>
  );

  if (type === 'list') {
    const handleClickEvent = () => {
      if (stockItem) {
        dispatch(setSelectedStockItem(stockItem));
      }
    }

    return (
      <>
        {headlineElement}
        <Link
          onClick={handleClickEvent}
          to={AppRoutes.STOCK_ITEM_DETAILS}>
          {stockItemElement}
        </Link>
      </>
    )
  }

  if (type === 'details') {
    return (
      <>
        {headlineElement}
        {stockItemElement}
        <Link to={AppRoutes.STOCK_ITEM_UPDATE}>{text.stockItemUpdateLinkLabel}</Link>
        <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
      </>
    );
  }

  const globalErrorElement = (<div>{globalError}</div>)
  const disabled =
    isInProgress
    || localMinimumQuantity === undefined
    || localMinimumQuantityError !== ''
    || !localName
    || localNameError !== ''
    || localQuantity === undefined
    || localQuantityError !== '';

  if (type === 'create') {
    const handleCreateSubmit = () => {
      if (!create
        || localMinimumQuantity === undefined
        || !localName
        || localQuantity === undefined
        || disabled) {
        return;
      }

      const stockItem: ICreateStockItem = {
        minimumQuantity: localMinimumQuantity,
        name: localName,
        quantity: localQuantity
      };

      create(stockItem, isSubmitAndNew);
    }

    return (
      <>
        {headlineElement}
        {globalErrorElement}
        <Form
          onSubmit={handleCreateSubmit}
        >
          {stockItemElement}
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

  if (type === 'update') {
    const handleUpdateSubmit = () => {
      if (!update
        || localMinimumQuantity === undefined
        || !localName
        || localQuantity === undefined
        || disabled) {
        return;
      }

      const stockItem: ICreateStockItem = {
        minimumQuantity: localMinimumQuantity,
        name: localName,
        quantity: localQuantity
      };

      update(stockItem);
    }

    return (
      <>
        {headlineElement}
        {globalErrorElement}
        <Form onSubmit={handleUpdateSubmit}>
          {stockItemElement}
          <Submit
            disabled={disabled}
            id='StockItemUpdateSubmit'
            label={text.stockItemUpdateSubmitLabel}
          />
        </Form>
        <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
      </>
    )
  }

  throw new Error(`Unknown type ${type}`);
}

export default StockItem;