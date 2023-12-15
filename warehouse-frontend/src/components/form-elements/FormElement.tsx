const FormElement = ({
    autoFocus,
    disabled,
    height,
    id,
    label,
    max,
    maxLength,
    min,
    minLength,
    pattern,
    placeholder,
    required,
    setValue,
    size,
    step,
    title,
    type,
    value,
    width
} : {
    autoFocus?: boolean,
    disabled?: boolean,
    height?: number,
    id: string,
    label: string,
    max?: string|number,
    maxLength?: number,
    min?: string|number,
    minLength?: number,
    pattern?: string,
    placeholder?: string,
    size?: number,
    required?: boolean,
    setValue: (value: string) => void,
    step?: number,
    title?: string,
    type?: 'password' | 'text',
    value?: string,
    width?: number
}) => {
    const onChange = (e: React.FormEvent<HTMLInputElement>) => {
        if (setValue) {
            setValue(e.currentTarget.value);
        }
    }

    return (
        <>
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