import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import Quantity from './Quantity';

const MinimumQuantity = ({
  error,
  id = 'minimumQuantity',
  max = Validation.STOCK_ITEM_MINIMUM_QUANTITY_MAX,
  min = Validation.STOCK_ITEM_MINIMUM_QUANTITY_MIN,
  setError,
  setValue,
  text,
  value,
  label = text.stockItemMinimumQuantityLabel,
  maxError = text.stockItemMinimumQuantityTooLarge,
  minError = text.stockItemMinimumQuantityTooSmall,
}: {
  error?: string,
  id?: string,
  label?: string,
  max?: number,
  maxError?: (maximum: number) => string,
  min?: number,
  minError?: (minimum: number) => string,
  setError?: (value: string) => void,
  setValue?: (value: string) => void,
  text: IText,
  value: string
}) => {
  return (
    <Quantity
      error={error}
      id={id}
      label={label}
      max={max}
      maxError={maxError}
      min={min}
      minError={minError}
      setError={setError}
      setValue={setValue}
      text={text}
      value={value}
    />
  )
}

export default MinimumQuantity;
