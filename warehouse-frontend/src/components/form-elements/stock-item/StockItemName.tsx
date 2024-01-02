import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";

const StockItemName = ({
  error,
  id = 'name',
  isReadOnly = false,
  maxLength = Validation.STOCK_ITEM_NAME_MAX_LENGTH,
  minLength = Validation.STOCK_ITEM_NAME_MIN_LENGTH,
  setError,
  setValue,
  text,
  value,
  label = text.stockItemNameLabel,
  maxLengthError = text.stockItemNameTooLongError,
  minLengthError = text.stockItemNameTooShortError,
}: {
  error?: string,
  id?: string,
  isReadOnly?: boolean,
  label?: string,
  maxLength?: number,
  maxLengthError?: (maxLength: number) => string,
  minLength?: number,
  minLengthError?: (minLength: number) => string,
  setError?: (value: string) => void,
  setValue?: (value: string) => void,
  text: IText,
  value?: string
}) => {
  return (
    <FormElement
      error={error}
      id={id}
      isReadOnly={isReadOnly}
      label={label}
      maxLength={maxLength}
      maxLengthError={maxLengthError}
      minLength={minLength}
      minLengthError={minLengthError}
      setError={setError}
      setValue={setValue ? (value) => setValue(value as string) : undefined}
      type="text"
      value={value}
    />
  )
}

export default StockItemName;
