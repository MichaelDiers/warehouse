import IText from "../../../text/text";
import Validation from '../../../types/validation-enum';
import FormElement from "../generic/FormElement";
import { validate as uuidValidate, version as uuidVersion } from 'uuid';

const InvitationCode = ({
  error,
  id = 'invitationCode',
  maxLength = Validation.INVITATION_CODE_MAX_LENGTH,
  minLength = Validation.INVITATION_CODE_MIN_LENGTH,
  setError,
  setValue,
  text,
  value,
  label = text.invitationCodeLabel,
  maxLengthError = text.invitationCodeTooLongError,
  minLengthError = text.invitationCodeTooShortError,
  formatError = text.invitationCodeFormatError
}: {
  error?: string,
  id?: string,
  formatError?: string,
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
  const additionalValidation = (value: string) => {
    if (!formatError || (uuidValidate(value) && uuidVersion(value) === 4)) {
      return '';
    }

    return formatError;
  }

  const autoComplete = (newValue: string) => {
    if (value && value.length > newValue.length) {
      return newValue;
    }

    const update = newValue.replace(/-+/g, '-');
    switch (update.length) {
      case 8:
      case 13:
      case 18:
      case 23:
        return `${update}-`.replace('--', '-');
      default:
        return update;
    }
  }

  return (
    <FormElement
      additionalValidation={additionalValidation}
      autoComplete={autoComplete}
      error={error}
      id={id}
      label={label}
      maxLength={maxLength}
      maxLengthError={maxLengthError}
      minLength={minLength}
      minLengthError={minLengthError}
      setError={setError}
      setValue={(value) => setValue(value as string)}
      type="text"
      value={value}
    />
  )
}

export default InvitationCode;
