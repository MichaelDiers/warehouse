import { 
  className as elementErrorClassName,
  ElementError 
} from './ElementError';

const FormElement = ({
  additionalValidation = (value: string) => '',
  autoComplete = (value: string) => value,
  autoFocus,
  disabled,
  error,
  height,
  id,
  isReadOnly = false,
  label,
  max,
  maxError,
  maxLength,
  maxLengthError,
  min,
  minError,
  minLength,
  minLengthError,
  minWidth,
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
  autoComplete?: (value: string) => string,
  autoFocus?: boolean,
  disabled?: boolean,
  error?: string,
  height?: number,
  id: string,
  isReadOnly?: boolean,
  label: string,
  max?: number,
  maxError?: (maximum: number) => string,
  maxLength?: number,
  maxLengthError?: (maxLength: number) => string,
  min?: number,
  minError?: (minumum: number) => string,
  minLength?: number,
  minLengthError?: (minLength: number) => string,
  minWidth?: number,
  pattern?: string,
  placeholder?: string,
  size?: number,
  required?: boolean,
  setError?: (value: string) => void,
  setValue?: (value: string | number) => void,
  step?: number,
  title?: string,
  type?: 'number' | 'password' | 'text',
  value?: string | number,
  width?: number
}) => {
  const onChange = (e: React.FormEvent<HTMLInputElement>) => {
    const value = autoComplete(e.currentTarget.value);
    if (setError) {
      if (maxLength && maxLengthError && value?.length > maxLength) {
        setError(maxLengthError(maxLength));
      }
      else if (minLength && minLengthError && (!value || value.length < minLength)) {
        setError(minLengthError(minLength));
      }
      else if (value && max && maxError && Number.isInteger(value) && Number(value) > max) {
        setError(maxError(Number(value)));
      }
      else if (value && min && minError && Number.isInteger(value) && Number(value) < min) {
        setError(minError(Number(value)));
      }
      else {
        setError(additionalValidation(value));
      }
    }

    if (setValue) {
      setValue(value);
    }
  }

  const className = `${error ? elementErrorClassName : ''} ${minWidth ? `min-width-${minWidth}` : ''}`;

  return (
    <div className='form-element'>
      <label htmlFor={id} className={error ? elementErrorClassName : ''}>{label}</label>
      <ElementError error={error}/>
      <input
        autoFocus={autoFocus}
        className={className}
        disabled={disabled || isReadOnly}
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
        readOnly={!setValue}
        required={required}
        size={size}
        step={step}
        title={title}
        type={type}
        value={value}
        width={width}
      />
    </div>
  )
}

export default FormElement;