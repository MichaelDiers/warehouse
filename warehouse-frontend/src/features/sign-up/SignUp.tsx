import { useState } from "react";
import IText from "../../text/text";
import { ISignUpRequest, useSignUpMutation } from "./sign-up-api-slice";
import { v4 } from 'uuid';
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { selectUser } from "../../app/selectors";
import { updateUserThunk } from "../../app/user-slice";
import { reset } from '../../app/options-slice';
import { SignUpForm } from './SignUpForm';

export function SignUp({
  text
}: {
  text: IText
}) {
  const [signUp, { status }] = useSignUpMutation();

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
          setError('Unable to sign up. Internal error.');
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
            case 409:
              setError('User already exists');
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
    <SignUpForm
      disabled={disabled}
      displayName={displayName}
      displayNameError={displayNameError}
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
  )
}
