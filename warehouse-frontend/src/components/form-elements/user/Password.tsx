import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";

const Password = ({
  additionalValidation = (value: string) => '',
  error,
  id = 'password',
  label,
  maxLength = Validation.PASSWORD_MAX_LENGTH,
  minLength = Validation.PASSWORD_MIN_LENGTH,
  setError,
  setValue,
  text,
  value,
  maxLengthError = text.passwordTooLongError,
  minLengthError = text.passwordTooShortError,
}: {
  additionalValidation?: (value: string) => string,
  error?: string,
  id?: string,
  label?: string,
  maxLength?: number,
  minLength?: number,
  setError?: (value: string) => void,
  setValue: (value: string) => void,
  text: IText,
  value: string,
  maxLengthError?: (maxLength: number) => string,
  minLengthError?: (minLength: number) => string,
}) => {
  const passwordValidation = (value: string) => {
    if (text.forbiddenPasswords.some(password => password.toLowerCase() === value.toLocaleLowerCase())) {
      return text.forbiddenPasswordsError;
    }

    return additionalValidation(value);
  }

  return (
    <FormElement
      additionalValidation={passwordValidation}
      error={error}
      id={id}
      label={label || text.passwordLabel}
      maxLength={maxLength}
      maxLengthError={maxLengthError}
      minLength={minLength}
      minLengthError={minLengthError}
      setError={setError}
      setValue={(password) => setValue(password as string)}
      type='password'
      value={value}
    />
  )
}

export default Password;
