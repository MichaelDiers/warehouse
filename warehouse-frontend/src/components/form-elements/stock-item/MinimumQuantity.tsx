import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import Quantity from './Quantity';

const MinimumQuantity = ({
  error,
  id = 'minimumQuantity',
  isReadOnly = false,
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
  isReadOnly?: boolean,
  label?: string,
  max?: number,
  maxError?: (maximum: number) => string,
  min?: number,
  minError?: (minimum: number) => string,
  setError?: (value: string) => void,
  setValue?: (value: number) => void,
  text: IText,
  value?: number
}) => {
  return (
    <Quantity
      error={error}
      id={id}
      isReadOnly={isReadOnly}
      label={label}
      max={max}
      maxError={maxError}
      min={min}
      minError={minError}
      setError={setError}
      setValue={setValue ? (value) => setValue(value as number) : undefined}
      text={text}
      value={value}
    />
  )
}

export default MinimumQuantity;
