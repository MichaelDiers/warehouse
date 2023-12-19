import { useState } from "react";
import IText from "../../text/text";
import { ISignInRequest, useSignInMutation } from "./sign-in-api-slice";
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectUser } from '../../app/selectors';
import { updateUserThunk } from '../../app/user-slice';
import { reset } from '../../app/options-slice';
import { SignInForm } from './SignInForm';

export function SignIn({
  text
}: {
  text: IText
}) {
  const [signIn, { status }] = useSignInMutation();

  const [password, setPassword] = useState('password');
  const [passwordError, setPasswordError] = useState('');
  const [id, setId] = useState('userName');
  const [idError, setIdError] = useState('');
  const [error, setError] = useState('');
  const disabled = status === QueryStatus.pending
    || !password
    || !id
    || idError !== ''
    || passwordError !== '';

  const user = useAppSelector(selectUser);

  const dispatch = useAppDispatch();

  const onSubmit = () => {
    setError('');

    const args: ISignInRequest = {
      password,
      id
    };

    signIn(args)
      .unwrap()
      .then((result) => {
        const { accessToken, refreshToken } = result;
        if (!accessToken || !refreshToken) {
          setError('Unable to sign in. Internal error.');
        }
        else {
          dispatch(updateUserThunk(accessToken, refreshToken));
          dispatch(reset());
        }
      }).catch((err): void => {
        const { status } = err;
        if (status) {
          switch (status) {
            case 401:
              setError('Invalid invitation code');
              break;
            default:
              setError(JSON.stringify(err));
              break;
          }
        }
      });
  };

  if (user) {
    return (<Navigate to={AppRoutes.STOCK_ITEM_LIST} />)
  }

  return (
    <SignInForm
      disabled={disabled}
      id={id}
      idError={idError}
      onSubmit={onSubmit}
      password={password}
      passwordError={passwordError}
      setId={setId}
      setIdError={setIdError}
      setPassword={setPassword}
      setPasswordError={setPasswordError}
      text={text}
    />
  )
}
