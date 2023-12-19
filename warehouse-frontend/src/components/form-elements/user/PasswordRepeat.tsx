import IText from "../../../text/text";
import Password from './Password';

const PasswordRepeat = ({
  additionalValidation,
  error,
  id = 'passwordRepeat',
  maxLength,
  minLength,
  setError,
  setValue,
  text,
  value,
  label = text.passwordRepeatLabel,
  maxLengthError,
  minLengthError,
}: {
  additionalValidation?: (value: string) => string,
  error?: string,
  id?: string,
  maxLength?: number,
  minLength?: number,
  setError?: (error: string) => void,
  setValue: (value: string) => void,
  text: IText,
  value: string,
  label?: string,
  maxLengthError?: (maxLength: number) => string,
  minLengthError?: (minLength: number) => string,
}) => {
  return (
    <Password
      additionalValidation={additionalValidation}
      error={error}
      id={id}
      label={label}
      maxLength={maxLength}
      maxLengthError={maxLengthError}
      minLength={minLength}
      minLengthError={minLengthError}
      setError={setError}
      setValue={setValue}
      text={text}
      value={value}
    />
  )
}

export default PasswordRepeat;
