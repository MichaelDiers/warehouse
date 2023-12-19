const Submit = ({
  autoFocus,
  disabled,
  height,
  id,
  label,
  onClick,
  type = 'submit',
  width
}: {
  autoFocus?: boolean,
  disabled?: boolean,
  height?: number,
  id: string,
  label: string,
  onClick?: () => void,
  type?: 'submit',
  width?: number
}) => {
  return (
    <>
      <input
        autoFocus={autoFocus}
        disabled={disabled}
        height={height}
        id={id}
        onClick={onClick}
        type={type}
        value={label}
        width={width}
      />
    </>
  )
}

export default Submit;
