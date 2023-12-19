import IText from "../../text/text";
import Validation from '../../types/validation-enum';
import FormElement from "./FormElement";

const Quantity = ({
  error,
  id = 'quantity',
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
    <FormElement
      error={error}
      id={id}
      label={label}
      max={max}
      maxError={maxError}
      min={min}
      minError={minError}
      setError={setError}
      setValue={setValue}
      type="text"
      value={value}
    />
  )
}

export default Quantity;
