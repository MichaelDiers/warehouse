import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";

const UserName = ({
  error,
  id = 'userName',
  maxLength = Validation.USERNAME_MAX_LENGTH,
  minLength = Validation.USERNAME_MIN_LENGTH,
  setError,
  setValue,
  text,
  value,
  label = text.userNameLabel,
  maxLengthError = text.userNameTooLongError,
  minLengthError = text.userNameTooShortError,
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
      setValue={(userName) => setValue(userName as string)}
      type="text"
      value={value}
    />
  )
}

export default UserName;
