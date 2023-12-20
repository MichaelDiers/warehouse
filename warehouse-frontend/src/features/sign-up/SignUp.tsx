import { useState } from "react";
import { ISignUpRequest, useSignUpMutation } from "./sign-up-api-slice";
import { v4 } from 'uuid';
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { selectText, selectUser } from "../../app/selectors";
import { updateUserThunk } from "../../app/user-slice";
import { reset } from '../../app/options-slice';
import { SignUpForm } from './SignUpForm';
import { InProgressIndicator } from '../../components/InProgress';
import ApplicationError from '../../types/application-error';

export function SignUp() {
  const [signUp, { status }] = useSignUpMutation();
  const text = useAppSelector(selectText);

  const [displayName, setDisplayName] = useState(v4());
  const [displayNameError, setDisplayNameError] = useState('');
  const [invitationCode, setInvitationCode] = useState('efdf22de-fbde-4a7f-b864-51858644399c');
  const [invitationCodeError, setInvitationCodeError] = useState('');
  const [password, setPassword] = useState('password');
  const [passwordError, setPasswordError] = useState('');
  const [passwordRepeat, setPasswordRepeat] = useState(password);
  const [passwordRepeatError, setPasswordRepeatError] = useState(passwordError);
  const [id, setId] = useState('userName');
  const [idError, setIdError] = useState('');
  const [error, setError] = useState('');
  const disabled = status === QueryStatus.pending
    || passwordError !== ''
    || passwordRepeatError !== ''
    || !password
    || !id
    || idError !== ''
    || !passwordRepeat
    || !invitationCode
    || invitationCodeError !== ''
    || !displayName
    || displayNameError !== '';

  const user = useAppSelector(selectUser);
  const validatePassword = (value: string) => {
    if (value !== passwordRepeat) {
      return text.passwordMismatchError;
    } else {
      if (passwordRepeatError === text.passwordMismatchError) {
        setPasswordRepeatError('');
      }

      return '';
    }
  }

  const validatePasswordRepeat = (value: string) => {
    if (value !== password) {
      return text.passwordMismatchError;
    } else {
      if (passwordError === text.passwordMismatchError) {
        setPasswordError('');
      }

      return '';
    }
  }

  const dispatch = useAppDispatch();

  const onSubmit = () => {
    setError('');

    const args: ISignUpRequest = {
      displayName,
      invitationCode,
      password,
      id
    };

    signUp(args)
      .unwrap()
      .then((result) => {
        const { accessToken, refreshToken } = result;
        if (!accessToken || !refreshToken) {
          setError(text.signUp500_1);
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
              case 400:
                setError(text.signUp400);
                break;
              case 401:
                setError(text.signUp401);
                break;
              case 403:
                setError(text.signUp403);
                break;
              case 409:
                setError(text.signUp409);
                break;
              default:
                setError(text.signUp500_2);
                break;
            }
          } else {
            setError(text.signUp500_3);
          }
        }
      });
  };

  if (user) {
    return (<Navigate to={AppRoutes.STOCK_ITEM_LIST} />)
  }

  return (
    <InProgressIndicator isInProgress={status === QueryStatus.pending}>
      <SignUpForm
        disabled={disabled}
        displayName={displayName}
        displayNameError={displayNameError}
        error={error}
        id={id}
        idError={idError}
        invitationCode={invitationCode}
        invitationCodeError={invitationCodeError}
        onSubmit={onSubmit}
        password={password}
        passwordError={passwordError}
        passwordRepeat={passwordRepeat}
        passwordRepeatError={passwordRepeatError}
        setDisplayName={setDisplayName}
        setDisplayNameError={setDisplayNameError}
        setId={setId}
        setIdError={setIdError}
        setInvitationCode={setInvitationCode}
        setInvitationCodeError={setInvitationCodeError}
        setPassword={setPassword}
        setPasswordError={setPasswordError}
        setPasswordRepeat={setPasswordRepeat}
        setPasswordRepeatError={setPasswordRepeatError}
        validatePassword={validatePassword}
        validatePasswordRepeat={validatePasswordRepeat}
        text={text}
      />
    </InProgressIndicator>
  )
}
