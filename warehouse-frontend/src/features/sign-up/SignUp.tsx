import { useState } from "react";
import Form from "../../components/form-elements/Form";
import Password from "../../components/form-elements/Password";
import SignUpSubmit from "../../components/form-elements/SignUpSubmit";
import UserName from "../../components/form-elements/UserName";
import IText from "../../text/text";
import PasswordRepeat from "../../components/form-elements/PasswordRepeat";
import InvitationCode from "../../components/form-elements/InvitationCode";
import { ISignUpRequest, useSignUpMutation } from "./sign-up-api-slice";
import DisplayName from "../../components/form-elements/DisplayName";
import { v4 } from 'uuid';
import { QueryStatus } from "@reduxjs/toolkit/dist/query";
import AppRoutes from "../../types/app-routes.enum";
import { Link, Navigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { selectUser } from "../../app/selectors";
import { updateUserThunk } from "../../app/user-slice";

export function SignUp({
  text
}: {
  text: IText
}) {
  const [signUp, { status }] = useSignUpMutation();

  const [displayName, setDisplayName] = useState(v4());
  const [invitationCode, setInvitationCode] = useState('efdf22de-fbde-4a7f-b864-51858644399c');
  const [password, setPassword] = useState('password');
  const [passwordError, setPasswordError] = useState('');
  const [passwordRepeat, setPasswordRepeat] = useState(password);
  const [passwordRepeatError, setPasswordRepeatError] = useState(passwordError);
  const [id, setId] = useState('userName');
  const [error, setError] = useState('');
  const disabled = status === QueryStatus.pending
    || passwordError !== ''
    || passwordRepeatError !== ''
    || !password
    || !id
    || !passwordRepeat
    || !invitationCode
    || !displayName;

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
    return (<Navigate to={AppRoutes.SHOPPING_LIST} />)
  }

  return (
    <>
      <Form onSubmit={onSubmit}>
        <div></div>
        <label>ERROR</label>
        <div>{error}</div>
        <div></div>
        <label>status</label>
        <div>{status}</div>

        <UserName
          setValue={setId}
          text={text}
          value={id}
        />
        <DisplayName
          setValue={setDisplayName}
          text={text}
          value={displayName}
        />
        <Password
          additionalValidation={validatePassword}
          error={passwordError}
          setError={setPasswordError}
          setValue={setPassword}
          text={text}
          value={password}
        />
        <PasswordRepeat
          additionalValidation={validatePasswordRepeat}
          error={passwordRepeatError}
          setError={setPasswordRepeatError}
          setValue={setPasswordRepeat}
          text={text}
          value={passwordRepeat}
        />
        <InvitationCode
          setValue={setInvitationCode}
          text={text}
          value={invitationCode}
        />
        <SignUpSubmit
          disabled={disabled}
          text={text} />
      </Form>
      <Link to={AppRoutes.SIGN_IN}>
        sign in
      </Link>
    </>
  )
}
