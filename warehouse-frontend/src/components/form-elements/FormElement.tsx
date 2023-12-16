const FormElement = ({
  additionalValidation = (value: string) => '',
  autoFocus,
  disabled,
  error,
  height,
  id,
  label,
  max,
  maxLength,
  maxLengthError,
  min,
  minLength,
  minLengthError,
  pattern,
  placeholder,
  required,
  setError,
  setValue,
  size,
  step,
  title,
  type,
  value,
  width
}: {
  additionalValidation?: (value: string) => string,
  autoFocus?: boolean,
  disabled?: boolean,
  error?: string,
  height?: number,
  id: string,
  label: string,
  max?: string | number,
  maxLength?: number,
  maxLengthError?: (maxLength: number) => string,
  min?: string | number,
  minLength?: number,
  minLengthError?: (minLength: number) => string,
  pattern?: string,
  placeholder?: string,
  size?: number,
  required?: boolean,
  setError?: (value: string) => void,
  setValue: (value: string) => void,
  step?: number,
  title?: string,
  type?: 'password' | 'text',
  value?: string,
  width?: number
}) => {
  const onChange = (e: React.FormEvent<HTMLInputElement>) => {
    const value = e.currentTarget.value;
    if (setError) {
      if (maxLength && maxLengthError && value?.length > maxLength) {
        setError(maxLengthError(maxLength));
      }
      else if (minLength && minLengthError && (!value || value.length < minLength)) {
        setError(minLengthError(minLength));
      }
      else {
        setError(additionalValidation(value));
      }
    }

    if (setValue) {
      setValue(e.currentTarget.value);
    }
  }

  return (
    <>
      <div>{error}</div>
      <label htmlFor={id}>{label}</label>
      <input
        autoFocus={autoFocus}
        disabled={disabled}
        height={height}
        id={id}
        max={max}
        maxLength={maxLength}
        min={min}
        minLength={minLength}
        name={id}
        onChange={onChange}
        pattern={pattern}
        placeholder={placeholder}
        required={required}
        size={size}
        step={step}
        title={title}
        type={type}
        value={value}
        width={width}
      />
    </>
  )
}

export default FormElement;