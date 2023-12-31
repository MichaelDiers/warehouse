import Error from './Error';

const className: string = 'error-element';

const ElementError = ({
  error,
} : {
  error?: string
}) => {
  return Error({ className, error });
}

export {
  className,
  ElementError,
}
