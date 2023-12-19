import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";

const DisplayName = ({
  error,
  id = 'displayName',
  maxLength = Validation.DISPLAY_NAME_MAX_LENGTH,
  minLength = Validation.DISPLAY_NAME_MIN_LENGTH,
  setError,
  setValue,
  text,
  value,
  label = text.displayName,
  maxLengthError = text.displayNameTooLongError,
  minLengthError = text.displayNameTooShortError,
}: {
  error?: string,
  id?: string,
  label?: string,
  maxLength?: number,
  maxLengthError?: (maxLength: number) => string,
  minLength?: number,
  minLengthError?: (minLength: number) => string,
  setError?: (value: string) => void,
  setValue: (value: string) => void,
  text: IText,
  value: string
}) => {
  return (
    <FormElement
      error={error}
      id={id}
      label={label}
      maxLength={maxLength}
      maxLengthError={maxLengthError}
      minLength={minLength}
      minLengthError={minLengthError}
      setError={setError}
      setValue={setValue}
      type="text"
      value={value}
    />
  )
}

export default DisplayName;
