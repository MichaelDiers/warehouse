import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";

const Quantity = ({
  error,
  id = 'quantity',
  isReadOnly = false,
  max = Validation.STOCK_ITEM_QUANTITY_MAX,
  min = Validation.STOCK_ITEM_QUANTITY_MIN,
  setError,
  setValue,
  text,
  value,
  label = text.stockItemQuantityLabel,
  maxError = text.stockItemQuantityTooLarge,
  minError = text.stockItemQuantityTooSmall,
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
    <FormElement
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
      type={isReadOnly ? 'text' : 'number'}
      value={value}
    />
  )
}

export default Quantity;
