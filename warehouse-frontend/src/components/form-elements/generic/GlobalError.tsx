import Error from './Error';

const GlobalError = ({
  error,
} : {
  error?: string
}) => {
  return Error({ className: 'error-global', error });
}

export default GlobalError;
