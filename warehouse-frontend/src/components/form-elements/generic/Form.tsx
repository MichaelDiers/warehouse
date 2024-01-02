import GlobalError from './GlobalError';

const Form = ({
  className,
  error,
  header,
  id,
  children,
  onSubmit
}: {
  className?: string,
  error?: string,
  header?: string,
  id?: string,
  children: string | JSX.Element | JSX.Element[],
  onSubmit: () => void
}): JSX.Element => {
  const onFormSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    onSubmit();
  };

  const headerElement = header ? (<h2>{header}</h2>) : (<></>);
  return (
    <form
      className={className}
      id={id}
      onSubmit={onFormSubmit}>
      {headerElement}
      <GlobalError error={error} />
      {children}
    </form>
  )
}

export default Form;
