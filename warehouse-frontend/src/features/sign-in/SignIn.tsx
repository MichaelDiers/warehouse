import { useState } from "react";
import { ISignInRequest, useSignInMutation } from "./sign-in-api-slice";
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Link, Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectText, selectUser } from '../../app/selectors';
import { updateUserThunk } from '../../app/user-slice';
import { reset } from '../../app/options-slice';
import { SignInForm } from './SignInForm';
import { InProgressIndicator } from '../../components/InProgress';
import ApplicationError from '../../types/application-error';

export function SignIn() {
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
  const text = useAppSelector(selectText);

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
          setError(text.signIn500_1);
        }
        else {
          dispatch(updateUserThunk(accessToken, refreshToken));
          dispatch(reset());
        }
      }).catch((err): void => {
        if (err.name === ApplicationError.name) {
          setError(err.message);
        } else {
          const { status } = err;
          if (status) {
            switch (status) {
              case 400: // bad req
                setError(text.signIn400);
                break;
              case 401:
                setError(text.signIn401);
                break;
              case 403:
                setError(text.signIn403);
                break;
              case 404:
                setError(text.signIn404);
                break;
              default:
                setError(text.signIn500_2);
                break;
            }
          } else {
            setError(text.signIn500_3);
          }
        }
      });
  };

  if (user) {
    return (<Navigate to={AppRoutes.STOCK_ITEM_LIST} />)
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <div className='grid-large'>
        <SignInForm
          disabled={disabled}
          error={error}
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
        <Link to={AppRoutes.SIGN_UP}>{text.genericSignUpLink}</Link>
      </div>
    </InProgressIndicator>
  )
}
