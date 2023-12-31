const Error = ({
  className,
  error,
} : {
  className: string,
  error?: string
}) => {
  if (!error) {
    return (<></>);
  }

  return (
    <div className={className}>
      {error}
    </div>
  )
}

export default Error;
