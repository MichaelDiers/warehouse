import IText from '../../../text/text';
import MinimumQuantity from './MinimumQuantity';
import Quantity from './Quantity';
import StockItemName from './StockItemName';

const StockItem = ({
  isReadOnly,
  minimumQuantity,
  minimumQuantityError,
  name,
  nameError,
  quantity,
  quantityError,
  setMinimumQuantity,
  setMinimumQuantityError,
  setName,
  setNameError,
  setQuantity,
  setQuantityError,
  text,
}: {
  isReadOnly?: boolean,
  minimumQuantity?: number,
  minimumQuantityError?: string,
  name?: string,
  nameError?: string,
  quantity?: number,
  quantityError?: string,
  setMinimumQuantity: (minumumQuantity: number) => void,
  setMinimumQuantityError: (minumumQuantityError: string) => void,
  setName: (name: string) => void,
  setNameError: (nameError: string) => void,
  setQuantity: (quantity: number) => void,
  setQuantityError: (quantityError: string) => void,
  text: IText,
}) => {
  return (
    <>
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
    </>
  );
}

export default StockItem;
