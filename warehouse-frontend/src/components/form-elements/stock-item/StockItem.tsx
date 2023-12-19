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

const StockItem = ({
  create,
  detailsUrl,
  globalError,
  headlineText,
  isInProgress,
  minimumQuantity,
  name,
  quantity,
  text
}: {
  create?: (stockItem: ICreateStockItem, isSubmitAndNew: boolean) => void,
  detailsUrl?: string,
  globalError?: string,
  headlineText?: string,
  isInProgress?: boolean,
  minimumQuantity?: number,
  minimumQuantityError?: string,
  name?: string
  quantity?: number,
  text: IText
}) => {
  const [localMinimumQuantity, setLocalMinimumQuantity] = useState(minimumQuantity || 0);
  const [localMinimumQuantityError, setLocalMinimumQuantityError] = useState('');
  const [localName, setLocalName] = useState(name || '');
  const [localNameError, setLocalNameError] = useState('');
  const [localQuantity, setLocalQuantity] = useState(quantity || 0);
  const [localQuantityError, setLocalQuantityError] = useState('');

  const [isSubmitAndNew, setIsSubmitAndNew] = useState<boolean>(false);

  const isReadOnly = !create;

  const createSubmitDisabled =
    isInProgress
    || localMinimumQuantity === undefined
    || localMinimumQuantityError !== ''
    || !localName
    || localNameError !== ''
    || localQuantity === undefined
    || localQuantityError !== '';

  const handleCreateSubmit = () => {
    if (!create
      || localMinimumQuantity === undefined
      || localName === undefined
      || localQuantity === undefined) {
      return;
    }

    const stockItem: ICreateStockItem = {
      minimumQuantity: localMinimumQuantity,
      name: localName,
      quantity: localQuantity
    };

    create(stockItem, isSubmitAndNew);
  }

  const headline = (headlineText ? <h1>{headlineText}</h1> : <></>);

  const stockItem = (
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

  if (detailsUrl) {
    return (
      <>
        {headline}
        <Link to={detailsUrl}>{stockItem}</Link>
      </>
    )
  }

  if (create) {
    return (
      <>
        {headline}
        <div>{globalError}</div>
        <Form
          onSubmit={handleCreateSubmit}
        >
          {stockItem}
          <Submit
            disabled={createSubmitDisabled}
            id='StockItemCreateSubmit'
            label={text.stockItemCreateSubmitLabel}
            onClick={() => setIsSubmitAndNew(false)}
          />
          <Submit
            disabled={createSubmitDisabled}
            id='StockItemCreateAndNewSubmit'
            label={text.stockItemCreateSubmitAndNewLabel}
            onClick={() => setIsSubmitAndNew(true)}
          />
        </Form>
        <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
      </>
    )
  }

  return (
    <>
      {headline}
      {stockItem}
      <Link to={AppRoutes.STOCK_ITEM_LIST}>{text.genericBackLabel}</Link>
    </>
  );
}

export default StockItem;